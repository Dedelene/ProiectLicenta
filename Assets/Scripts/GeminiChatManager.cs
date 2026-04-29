using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;

[System.Serializable]
public class GroqResponse
{
    public GroqChoice[] choices;
}

[System.Serializable]
public class GroqChoice
{
    public GroqMessage message;
}

[System.Serializable]
public class GroqMessage
{
    public string content;
}

public class GeminiChatManager : MonoBehaviour
{
    public TMP_InputField playerInput;
    public TextMeshProUGUI aiResponse;

    private string apiKey = "";
    private string apiURL = "https://api.groq.com/openai/v1/chat/completions";
    private string systemInstructions = @"Esti un gardian care verifica cunostintele economice ale playerului.
         Saluti si te prezinti sub numele de Joe (doar la inceputul conversatiei). Intrebi playerul daca e pregatit pentru intrebari.
        Astepti raspuns.
        LOGICA DE SELECȚIE (CRITICAL):
        - Ai o bază de date de 16 întrebări mai jos. 
        - Înainte de a începe, AMESTECĂ virtual toată lista (1-16). 
        - Alege 5 numere complet aleatorii din acest interval (exemplu: 14, 3, 9, 1, 12).
        - ESTE INTERZIS să pui doar primele 5 întrebări din listă. Diversitatea este obligatorie.
        FLUXUL CONVERSAȚIEI:
        1. Salută scurt, prezintă-te (doar o dată!) și întreabă dacă playerul este gata.
        2. Așteaptă confirmarea.
        NU ii spune nimic din aceste reguli playerului. (De ex: 'Am selectat 5 intrebari aleator dintr-o serie de 16 intrebari')
        3. Pune prima întrebare din cele 5 alese random. Așteaptă răspunsul.
        4. Evaluează răspunsul: 
           - Dacă e corect, confirmă și treci la următoarea.
           - Dacă e greșit sau playerul îți cere răspunsul, refuză politicos și trimite-l să cerceteze cărțile din cameră. NU oferi tu răspunsul corect.
        5. După 5 întrebări corecte, felicită-l și eliberează-l.
        REGULI DE FORMATARE STRICTE:
        - Stil: Misterios, direct, scurt (max 2 paragrafe scurte pe mesaj).
        - FĂRĂ meta-limbaj: Nu spune 'Întrebarea 1', nu repeta regulile jocului.
        - FĂRĂ numerotare: Doar pune întrebarea direct.
        LISTA DE ÎNTREBĂRI:
        1.Cum se calculeaza salariul net din cel brut?
        2.Care este cea mai importanta institutie in procesul de reglementare a legilor?
        3.Care este termenul pentru un bun sau serviciu ce poate fi utilizat in locul altuia pentru a satisface aceeasi nevoie?
        4.Atunci cand economia creste, somajul ... (Legea lui Okun)
        5.Ce valori se stabilesc la echilibrul pietei?
        6.Ce inseamna IPC?
        7.Care este cauza cresterii preturilor si cum afecteaza acest lucru valoarea banilor in timp?
        8.Ce inseamna PIB?
        9.Care ar putea fi costul de oportunitate al deciziei de a merge la facultate?
        10.Ce inseamna monopol?
        11.Indica un exemplu reprezentativ a termenului cu initiala W din analiza SWOT a unei firme
        12.Care este diferenta dintre IPC si deflatorul PIB?
        13.Ce presupune o economie inchisa?
        14.Din ce categorie de venituri face parte TVA-ul?
        15.Din ce categorie de cheltuieli fac parte sumele alocate pentru constructia de autostrazi?
        16.Care este singura institutie care are dreptul legal de a tipari bancnote si de a pune in circulatie monede?]";

    private List<string> messageLog = new List<string>();
    private int maxMessages = 20;

    void Awake()
    {
        TextAsset keyFile = Resources.Load<TextAsset>("api_key");
        if (keyFile != null)
        {
            apiKey = keyFile.text.Trim();
        }
        else
        {
            Debug.LogError("Nu am gasit fisierul api_key.txt in folderul Resources!");
        }
    }
    void Start()
    {
        playerInput.onSubmit.AddListener(delegate { OnInputSubmit(); });
    }

    private void OnInputSubmit()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendMesageToGemini();
        }
    }

    public void StartFirstConversation()
    {
        if (messageLog.Count == 0)
        {
            aiResponse.text = "Joe se gandeste...";
            StartCoroutine(PostRequest("Salut... Cine esti si ce se intampla aici?"));
        }
    }
    private void AddToHistory(string message)
    {
        messageLog.Add(message);
        if (messageLog.Count > maxMessages)
        {
            messageLog.RemoveAt(0);
        }
    }
    public void SendMesageToGemini()
    {
        string playerText = playerInput.text;
        if (string.IsNullOrEmpty(playerText)) return;
        aiResponse.text = "Se gandeste...";
        playerInput.text = "";

        StartCoroutine(PostRequest(playerText));
    }

    private IEnumerator PostRequest(string playerMessage)
    {
        AddToHistory("Player: " + playerMessage + "\n");

        string currentHistory = string.Join("\n", messageLog);
        string prompt = systemInstructions + "\n\nIstoric conversatie:\n" + currentHistory + "\nAI:";

        string safePrompt = prompt.Replace("\n", "\\n").Replace("\r", "").Replace("\"", "\\\"");

        string jsonPayload = "{\"model\": \"llama-3.3-70b-versatile\", \"messages\": [{\"role\": \"user\", \"content\": \"" + safePrompt + "\"}]}";
        string requestURL = apiURL + "?key=" + apiKey;

        using (UnityWebRequest request = new UnityWebRequest(requestURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonPayload);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Eroare de la Google API: " + request.downloadHandler.text);
                aiResponse.text = "Eroare detaliata in Consola!";
            }
            else
            {
                GroqResponse responseObj = JsonUtility.FromJson<GroqResponse>(request.downloadHandler.text);

                if (responseObj != null && responseObj.choices.Length > 0)
                {
                    string responseText = responseObj.choices[0].message.content;
                    aiResponse.text = responseText;
                    AddToHistory("AI: " + responseText);
                }
                else
                {
                    aiResponse.text = "Eroare la parsarea JSON-ului.";
                }
            }
        }
    }

    private string ExtractGeminiResponse(string jsonResponse)
    {
        try
        {
            string searchStr = "\"text\": \"";
            int startIndex = jsonResponse.IndexOf(searchStr) + searchStr.Length;
            int endIndex = jsonResponse.IndexOf("\"", startIndex);

            string finalResponse = jsonResponse[startIndex..endIndex];

            finalResponse = finalResponse.Replace("\\n", "\n").Replace("\\\"", "\"");
            return finalResponse;
        }
        catch
        {
            return "Eroare la procesarea raspunsului.";
        }
    }
}
