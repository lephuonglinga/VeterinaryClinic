using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using VeterinaryClinic.DTOs;
using VeterinaryClinic.Models;

public class GeminiMedicationSuggestionService
{
    private const string EndPoint =
        "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent";

    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public GeminiMedicationSuggestionService()
    {
        _httpClient = new HttpClient();
        _apiKey = "AIzaSyAFu2h_c68uO4x2ZIWOYV_jlTMci8RazmQ";
    }

    /// <summary>
    /// Calls Gemini API synchronously with the prompt and returns the full raw JSON response as string.
    /// </summary>
    public string? GetGeminiResponse(string prompt)
    {
        var url = $"{EndPoint}?key={_apiKey}";

        var payload = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = prompt }
                    }
                }
            }
        };

        string jsonPayload = JsonSerializer.Serialize(payload);

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        request.Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        using (HttpResponseMessage response = _httpClient.Send(request))
        {
            response.EnsureSuccessStatusCode();
            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            return jsonResponse;
        }
    }

    /// <summary>
    /// Build an instruction prompt for Gemini based on patient background and medication list.
    /// </summary>
    public string BuildPrompt(
        string patientId,
        List<CaseBackgroundInfo> patientCases,
        List<PharmacyInventory> candidateMedications)
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are a veterinary medication engine.");
        sb.AppendLine();
        sb.AppendLine($"Given the patient's background and existing health cases, suggest up to 5 new medications for their prescription.");
        sb.AppendLine();
        sb.AppendLine("IMPORTANT:");
        sb.AppendLine("- You must respond with a strictly valid JSON array only.");
        sb.AppendLine("- No additional text, explanations, formatting, or markdown.");
        sb.AppendLine("- Do NOT include any wrappers like code fences.");
        sb.AppendLine("- The JSON array must follow this format exactly:");
        sb.AppendLine("[");
        sb.AppendLine("  {");
        sb.AppendLine("    \"MedicationId\": 123,");
        sb.AppendLine("    \"Reason\": \"Reason for recommending this medication.\",");
        sb.AppendLine("    \"Description\": \"Short description of this medication.\",");
        sb.AppendLine("    \"Quantity\": 5,");
        sb.AppendLine("    \"Frequency\": \"Twice daily\",");
        sb.AppendLine("    \"Route\": \"Oral\"");
        sb.AppendLine("  },");
        sb.AppendLine("  ...");
        sb.AppendLine("]");
        sb.AppendLine();
        //sb.AppendLine("- Only suggest medications the patient is not currently prescribed.");
        sb.AppendLine("- Take into account patient details such as diagnosis, species, breed, age, and sex.");
        sb.AppendLine("- Consider medication category, trade name, and expiration date.");
        sb.AppendLine("- Suggest appropriate quantity, frequency, and route of administration.");
        sb.AppendLine("- Keep the output concise and clear.");
        sb.AppendLine();
        sb.AppendLine($"Patient ID: {patientId}");
        sb.AppendLine();
        sb.AppendLine("Patient Cases:");
        foreach (var c in patientCases)
        {
            sb.AppendLine($"- CaseId: {c.CaseId}, Date: {c.CaseDate?.ToString("yyyy-MM-dd") ?? "N/A"}, Diagnosis: {c.DiagnosisName ?? "Unknown"}, Species: {c.SpeciesName ?? "Unknown"}, Breed: {c.BreedName ?? "Unknown"}, AgeGroup: {c.PatientAgeGroup ?? "Unknown"}, Sex: {c.PatientSex ?? "Unknown"}");
        }
        sb.AppendLine();
        sb.AppendLine("Candidate Medications:");
        foreach (var med in candidateMedications)
        {
            sb.AppendLine($"- MedicationId: {med.ItemId}, TradeName: {med.TradeName}, GenericName: {med.GenericName}, Category: {med.Category ?? "None"}, ExpirationDate: {med.ExpirationDate?.ToString("yyyy-MM-dd") ?? "N/A"}");
        }

        return sb.ToString();
    }

    /// <summary>
    /// Calls Gemini API and parses medication suggestions from the JSON response.
    /// </summary>
    public List<GeminiMedicationSuggestionDTO> GetMedicationSuggestions(
        string prompt,
        bool enableLogging = false)
    {
        string? responseJson = GetGeminiResponse(prompt);
        if (responseJson is null)
            return new List<GeminiMedicationSuggestionDTO>();

        try
        {
            using JsonDocument doc = JsonDocument.Parse(responseJson);
            var contentElement = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrWhiteSpace(contentElement))
                return new List<GeminiMedicationSuggestionDTO>();

            // Extract JSON array substring within text (between first '[' and last ']')
            int start = contentElement.IndexOf('[');
            int end = contentElement.LastIndexOf(']');
            if (start == -1 || end == -1 || end <= start)
                return new List<GeminiMedicationSuggestionDTO>();

            string jsonArray = contentElement.Substring(start, end - start + 1);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var suggestions = JsonSerializer.Deserialize<List<GeminiMedicationSuggestionDTO>>(jsonArray, options) ?? new List<GeminiMedicationSuggestionDTO>();

            if (enableLogging)
            {
                Console.WriteLine("Gemini Parsed Medication Suggestions:");
                foreach (var s in suggestions)
                {
                    Console.WriteLine($"MedicationId: {s.MedicationId}, Reason: {s.Reason}, Description: {s.Description}");
                }
            }

            return suggestions;
        }
        catch (Exception ex)
        {
            if (enableLogging)
            {
                Console.WriteLine("Error parsing Gemini response:");
                Console.WriteLine(ex.Message);
            }
            return new List<GeminiMedicationSuggestionDTO>();
        }
    }
        public string? GetRawResponse(string prompt)
    {
        return GetGeminiResponse(prompt);
    }

    public List<GeminiMedicationSuggestionDTO> ParseSuggestions(string rawResponse)
    {
        if (string.IsNullOrWhiteSpace(rawResponse)) return new List<GeminiMedicationSuggestionDTO>();

        try
        {
            using JsonDocument doc = JsonDocument.Parse(rawResponse);
            var content = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            if (string.IsNullOrWhiteSpace(content))
                return new List<GeminiMedicationSuggestionDTO>();

            int start = content.IndexOf('[');
            int end = content.LastIndexOf(']');

            if (start == -1 || end == -1 || end <= start)
                return new List<GeminiMedicationSuggestionDTO>();

            var jsonArray = content.Substring(start, end - start + 1);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<List<GeminiMedicationSuggestionDTO>>(jsonArray, options) ?? new List<GeminiMedicationSuggestionDTO>();
        }
        catch
        {
            return new List<GeminiMedicationSuggestionDTO>();
        }
    }

}
