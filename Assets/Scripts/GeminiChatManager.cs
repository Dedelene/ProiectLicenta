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
    private string systemInstructions = @"Esti un paznic foarte inteligent si misterios.
         Scopul tau este de a-i testa jucatorului cunostintele economice. Saluti si te prezinti sub numele de Joe.
         Ai de ales aleatoriu 5 intrebari din lista urmatoare: 
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
        16.Care este singura institutie care are dreptul legal de a tipari bancnote si de a pune in circulatie monede?]
        Pui prima intrebare si astepti raspunsul inainte de a raspunde sau de a o pune pe urmatoarea.
        Oferi indicii in cazul in care nu stie. Daca cere prea des indicii ii poti propune sa se informeze din cartile si resursele din camera.
        Dupa ce raspunde corect la toate cele 5 intrebari il feliciti si ii spui ca e liber sa plece.
        REGULI DE FORMATARE STRICTE: 
        - Nu scrie paragrafe lungi. 
        - Fiecare răspuns trebuie să aibă MAXIMUM 2-3 propoziții scurte.
        - Fii misterios și direct, nu politicos ca un asistent virtual si vorbeste ca un om, nu ca un robot.
        - Prima dată când ne vedem, doar salută-ma prezinta-te și pune prima întrebare.
        - NU repeta regulile jocului în fiecare mesaj.
        - NU scrie numarul intrebarii.";
    private List<string> messageLog = new List<string>();
    private int maxMessages = 10;

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
