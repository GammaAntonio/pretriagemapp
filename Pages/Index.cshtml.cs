using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PreTriagemApp.Pages;

public class IndexModel : PageModel
{
    // Dados básicos
    [BindProperty] public int Age { get; set; }
    [BindProperty] public string Fever { get; set; } = "Não"; // Não / Baixa / Alta
    [BindProperty] public int Pain { get; set; } // 0-10

    // Respiratório / Otorrino
    [BindProperty] public bool Headache { get; set; }
    [BindProperty] public bool StuffyNose { get; set; }
    [BindProperty] public bool RunnyNose { get; set; }
    [BindProperty] public bool Sneezing { get; set; }
    [BindProperty] public bool SoreThroat { get; set; }
    [BindProperty] public bool Hoarseness { get; set; }
    [BindProperty] public bool Cough { get; set; }
    [BindProperty] public bool Wheezing { get; set; } // chiado
    [BindProperty] public bool EarPain { get; set; }
    [BindProperty] public bool FacialPain { get; set; } // dor/pressão na face
    [BindProperty] public bool ShortnessOfBreath { get; set; }
    [BindProperty] public bool ChestPain { get; set; }

    // Gastro
    [BindProperty] public bool Nausea { get; set; }
    [BindProperty] public bool Vomiting { get; set; }
    [BindProperty] public bool Diarrhea { get; set; }
    [BindProperty] public bool AbdominalPain { get; set; }

    // Urinário
    [BindProperty] public bool BurningUrination { get; set; }
    [BindProperty] public bool FrequentUrination { get; set; }

    // Pele / alergia
    [BindProperty] public bool SkinRash { get; set; }
    [BindProperty] public bool Itching { get; set; }
    [BindProperty] public bool SwellingFaceLips { get; set; } // ALERTA

    // Ortopedia
    [BindProperty] public bool BackPain { get; set; }
    [BindProperty] public bool JointPain { get; set; }

    // Olhos
    [BindProperty] public bool BlurredVision { get; set; }
    [BindProperty] public bool EyePain { get; set; }

    // Neuro / alertas
    [BindProperty] public bool Dizziness { get; set; }
    [BindProperty] public bool Fainting { get; set; } // ALERTA
    [BindProperty] public bool Confusion { get; set; } // ALERTA
    [BindProperty] public bool WeaknessOneSide { get; set; } // ALERTA

    public IActionResult OnPost()
    {
        // 1) Red flags (urgência alta)
        bool redFlag =
            ShortnessOfBreath ||
            ChestPain ||
            Fainting ||
            Confusion ||
            WeaknessOneSide ||
            SwellingFaceLips;

        if (redFlag)
        {
            return RedirectToPage("/Result", new
            {
                urgency = "ALTA",
                specialty = "Pronto-socorro / emergência",
                topic = "emergencia",
                hint = "Você marcou um sinal de alerta. Procure atendimento imediato."
            });
        }

        // 2) Base
        string topic = "geral";
        string specialty = "Clínica geral / UBS";
        string urgency = (Fever == "Alta" || Pain >= 7) ? "MODERADA" : "BAIXA";
        string hint = "Sem sinais de alerta. Comece por clínica geral/UBS.";

        // Respiratório/otorrino
        bool respiratory =
            StuffyNose || RunnyNose || Sneezing || SoreThroat || Cough || Hoarseness || FacialPain || EarPain;

        if (respiratory)
        {
            topic = "respiratorio";
            specialty = "Clínica geral / Otorrino";
            hint = "Sintomas respiratórios/otorrino. Se persistir ou piorar, procure avaliação.";
        }

        if (Wheezing)
        {
            topic = "respiratorio";
            specialty = "Clínica geral / Pneumologia";
            hint = "Chiado pode precisar de avaliação, especialmente se voltar com frequência.";
            urgency = "MODERADA";
        }

        // Gastro
        if (Nausea || Vomiting || Diarrhea || AbdominalPain)
        {
            topic = "gastro";
            specialty = "Clínica geral / Gastroenterologia";
            hint = "Sintomas gastrointestinais. Observe hidratação e evolução.";
        }

        // Urinário
        if (BurningUrination || FrequentUrination)
        {
            topic = "urinario";
            specialty = "Clínica geral / Urologia";
            hint = "Sintomas urinários. Avaliação pode ser necessária para investigar irritação/infecção.";
        }

        // Pele
        if (SkinRash || Itching)
        {
            topic = "pele";
            specialty = "Clínica geral / Dermatologia";
            hint = "Sintomas de pele/alergia. Observe gatilhos e evolução.";
        }

        // Olhos
        if (BlurredVision || EyePain)
        {
            topic = "olhos";
            specialty = "Oftalmologia";
            hint = "Sintomas oculares merecem avaliação, especialmente se forem novos.";
            urgency = "MODERADA";
        }

        // Ortopedia
        if (BackPain || JointPain)
        {
            topic = "ortopedia";
            specialty = "Clínica geral / Ortopedia";
            hint = "Dor musculoesquelética. Avalie esforço, postura e duração.";
        }

        // Neuro leve (sem red flags)
        if (Headache || Dizziness)
        {
            topic = topic == "geral" ? "neuro" : topic;
            hint = "Observe evolução. Se persistir ou piorar, procure clínica geral.";
        }

        return RedirectToPage("/Result", new { urgency, specialty, topic, hint });
    }
}
