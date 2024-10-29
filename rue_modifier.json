using System;
using Models.Core;
using Models.Interfaces;
using Models.PMF;
// I used chatGPT to arrive in this script and help from the APSIMx Q&A

namespace Models
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(Model))]
    public class Script : Model
    {
        [Link] private IClock Clock;
        [Link] private Sugarcane Sugarcane;
        [Link] private ISummary Summary;
        
       
  [Description("rue for plant:")]
    public string ruePlant { get; set; }
  [Description("rue for ratoon:")]
    public string rueRatoon { get; set; } // Initialize as needed

    [EventSubscribe("StartOfSimulation")]
    private void OnStartOfSimulation(object sender, EventArgs e)
    {
        Summary.WriteMessage(this, "StartOfSimulation event triggered.", MessageType.Information);

        var plant = Sugarcane.plant as SugarcaneSpeciesConstants;
        var ratoon = Sugarcane.ratoon as SugarcaneSpeciesConstants; 
        
        plant.rue = ruePlant;
        ratoon.rue= rueRatoon;
        Summary.WriteMessage(this, $"Plant rue values set at start: {string.Join(", ", plant.rue)}", MessageType.Information);
        Summary.WriteMessage(this, $"Ratoon rue values set at start: {string.Join(", ", ratoon.rue)}", MessageType.Information);
    }

    [EventSubscribe("DoManagement")]
    private void OnDoManagement(object sender, EventArgs e)
    {
        var plant = Sugarcane.plant as SugarcaneSpeciesConstants;
        var ratoon = Sugarcane.ratoon as SugarcaneSpeciesConstants;
        
        Summary.WriteMessage(this, $"Plant rue values during simulation: {string.Join(", ", plant.rue)}", MessageType.Information);
        Summary.WriteMessage(this, $"Ratoon rue values during simulation: {string.Join(", ", ratoon.rue)}", MessageType.Information);
        

    }

    [EventSubscribe("EndOfSimulation")]
    private void OnEndOfSimulation(object sender, EventArgs e)
    {
        var plant = Sugarcane.plant as SugarcaneSpeciesConstants;
        var ratoon = Sugarcane.ratoon as SugarcaneSpeciesConstants;
        
        Summary.WriteMessage(this, $"Plant rue values at end of simulation: {string.Join(", ", plant.rue)}", MessageType.Information);
        Summary.WriteMessage(this, $"Ratoon rue values at end of simulation: {string.Join(", ", ratoon.rue)}", MessageType.Information);
    }
    }
}
