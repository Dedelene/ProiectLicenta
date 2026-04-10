# Escape Room: The Price of Freedom

Acesta este un proiect de tip Escape Room 3D realizat în Unity. Jucătorul trebuie să evadeze din casa unui economist, rezolvând puzzle-uri care îi testează cunoștințele economice.

# Caracteristici

Gameplay: First-person interaction (explorare, colectare de indicii).
Sistem de Salvare: Progresul este salvat într-un fișier local JSON (poziție jucător, starea ușilor/obiectelor).
Dialog cu AI (Joe): Un personaj inteligent integrat via Groq API (LLaMA 3.3) care testează jucătorul cu întrebări dinamice și oferă indicii în timp real.
Puzzle-uri: Mecanici de tip tastatură numerică (Keypad), interacțiune cu obiecte (Note system) și inventar simplu.

# Tehnologii folosite

Motor grafic: Unity 2022.3+
Limbaj: C#
AI Integration: Groq API (pentru procesarea limbajului natural).
Persistență date: JSON Serialization.

# Instalare și Configurare

Deschide proiectul în Unity Hub.
Configurare API: Pentru ca sistemul de chat să funcționeze:
    1.Creează un folder numit Resources în Assets.
    2.Adaugă un fișier text numit api_key.txt.
    3.Lipește cheia ta de la Groq Console în acest fișier (fișierul este ignorat de Git pentru securitate).
Build and run.

# Credite și Asset-uri

Marea parte a modelelor 3D sunt preluate de pe Sketchfab sub licență Creative Commons Attribution sau Standard.
Modele 3D principale:
Agon Visionnaire Armchair - Nikolay Kudrin
Ficus Bonsai - Zgon
Bag Medieval - KIFIR
Chess Board - Mohammed.Adnan
Books & Paper AssetsYoung_Wizard, James.Moore, 3D_for_everyone
Newton Cradle - rickmaolly
Statue of Napoleon - Loïc Norgeot
Keypad System - Aya Dja
... și alte obiecte de decor (lămpi, rafturi, plante).
