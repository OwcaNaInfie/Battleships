# Opis działania API HallOfFameController

## GetTopThreePlayers

- **Opis**: Zwraca trzech najlepszych graczy na podstawie liczby wygranych.
- **Działanie**: Używa metody `TopThree` klasy `HallOfFame`.

## GetAllPlayerStatistics

- **Opis**: Zwraca pełną listę statystyk wszystkich graczy.
- **Działanie**: Używa metody `GetPlayerStatistics` klasy `HallOfFame`.

## AddOrUpdatePlayerStatistics

- **Opis**: Aktualizuje statystyki gracza na podstawie przesłanych danych (czy gracz wygrał lub przegrał grę).
- **Działanie**: Przyjmuje dane w formacie JSON, które są mapowane na klasę `UpdateStatisticsRequest`.

---

# Przykład żądań do API

## 1. Pobranie najlepszych graczy:

- **Metoda HTTP**: `GET`
- **Endpoint**: `/api/HallOfFame/TopThree`

**Odpowiedź (JSON):**

```json
[
    { "playerID": 1, "gamesPlayed": 20, "gamesWon": 15, "gamesLost": 5 },
    { "playerID": 2, "gamesPlayed": 25, "gamesWon": 12, "gamesLost": 13 },
    { "playerID": 3, "gamesPlayed": 10, "gamesWon": 8, "gamesLost": 2 }
]
```

---

## 2. Pobranie wszystkich statystyk:

- **Metoda HTTP**: `GET`
- **Endpoint**: `/api/HallOfFame/All`

**Odpowiedź (JSON):**

```json
[
    { "playerID": 1, "gamesPlayed": 20, "gamesWon": 15, "gamesLost": 5 },
    { "playerID": 2, "gamesPlayed": 25, "gamesWon": 12, "gamesLost": 13 },
    { "playerID": 3, "gamesPlayed": 10, "gamesWon": 8, "gamesLost": 2 },
    { "playerID": 4, "gamesPlayed": 5, "gamesWon": 2, "gamesLost": 3 }
]
```

---

## 3. Aktualizacja statystyk gracza:

- **Metoda HTTP**: `POST`
- **Endpoint**: `/api/HallOfFame/AddOrUpdate`

**Body (JSON):**

```json
{
    "playerID": 5,
    "isWin": true
}
```

**Odpowiedź:**

```json
"Player statistics updated successfully."