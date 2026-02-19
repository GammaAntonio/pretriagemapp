using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PreTriagemApp.Pages;

public class ResultModel : PageModel
{
    public string Urgency { get; set; } = "BAIXA";
    public string Specialty { get; set; } = "Clínica geral / UBS";
    public string Hint { get; set; } = "";
    public string Topic { get; set; } = "geral";

    public List<string> SpecificTips { get; set; } = new();
    public List<string> WhenToSeekCare { get; set; } = new();

    public void OnGet(string urgency, string specialty, string topic, string hint)
    {
        Urgency = urgency ?? "BAIXA";
        Specialty = specialty ?? "Clínica geral / UBS";
        Hint = hint ?? "";
        Topic = topic ?? "geral";

        BuildTips();
    }

    private void BuildTips()
    {
        WhenToSeekCare = new()
        {
            "Dor no peito forte ou falta de ar importante.",
            "Desmaio, confusão ou fraqueza em um lado do corpo.",
            "Febre alta persistente ou piora rápida."
        };

        SpecificTips = Topic switch
        {
            "respiratorio" => new()
            {
                "Descanso e hidratação ajudam.",
                "Evite fumaça e poeira.",
                "Procure avaliação se durar muitos dias."
            },

            "gastro" => new()
            {
                "Hidratação é essencial.",
                "Prefira alimentos leves.",
                "Observe sinais de desidratação."
            },

            "pele" => new()
            {
                "Evite produtos irritantes.",
                "Não coce a região.",
                "Observe se espalha."
            },

            _ => new()
            {
                "Observe evolução dos sintomas.",
                "Procure clínica geral se persistir."
            }
        };
    }
}
