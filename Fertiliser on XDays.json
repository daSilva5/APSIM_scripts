using Models.Interfaces;
using APSIM.Shared.Utilities;
using Models.Soils;
using Models.Soils.Nutrients;
using Models.PMF;
using Models.Core;
using System;

// I developed this script with help of ChatGPT
namespace Models
{
    [Serializable]
    [System.Xml.Serialization.XmlInclude(typeof(Model))]
    public class Script : Model
    {
        [Link] private Clock Clock;
        [Link] private Soil Soil; 
        [Link] private Fertiliser Fertiliser;
        [Link] private Summary Summary;
        [Link] private Sugarcane Sugarcane;

        [Link(ByName = true)]
        private ISolute NO3;

        [Link(ByName = true)]
        private ISolute NH4;

        // Days after sowing for planting
        [Description("Days After Sowing plant")]
        public double DaysAfterSowingPlant { get; set; }

        // Days after sowing for ratoon
        [Description("Days After Sowing ratoon")]
        public double DaysAfterSowingRatoon { get; set; }

        // Fertilization amounts
        [Description("Fertilization amount for planting (kg/ha)")]
        public double FertPlAmt { get; set; }

        [Description("Fertilization amount for ratoon (kg/ha)")]
        public double FertRatAmt { get; set; }

        // Type of fertilizer to apply for planting
        [Description("Type of fertilizer to apply for planting")]
        public Fertiliser.Types FertTypePlant { get; set; } = Fertiliser.Types.UreaN; // Default to UreaN

        // Type of fertilizer to apply for ratoon
        [Description("Type of fertilizer to apply for ratoon")]
        public Fertiliser.Types FertTypeRatoon { get; set; } = Fertiliser.Types.UreaN; // Default to UreaN

        [EventSubscribe("StartOfDay")]
        private void OnStartOfDay(object sender, EventArgs e)
        {
            double currentDaysAfterSowing = Sugarcane.DaysAfterSowing;
            int ratoonCount = Sugarcane.ratoon_no;

            // Harvesting logic based on days after sowing and ratoon number
            if ((currentDaysAfterSowing == DaysAfterSowingPlant && ratoonCount == 0) ||
                (currentDaysAfterSowing == DaysAfterSowingRatoon && ratoonCount > 0))
            {
                // Log harvesting action
                Summary.WriteMessage(this, $"Harvesting action triggered on day {currentDaysAfterSowing} for ratoon count {ratoonCount}.", MessageType.Information);
            }

            // Apply fertilizer based on ratoon number and days after sowing
            if (ratoonCount == 0 && currentDaysAfterSowing == DaysAfterSowingPlant)
            {
                Fertiliser.Apply(Amount: FertPlAmt, Depth: 80, Type: FertTypePlant);
                Summary.WriteMessage(this, $"Applied {FertPlAmt} kg/ha of {FertTypePlant} fertilizer for planting.", MessageType.Information);
            }
            else if (ratoonCount > 0 && currentDaysAfterSowing == DaysAfterSowingRatoon)
            {
                Fertiliser.Apply(Amount: FertRatAmt, Depth: 80, Type: FertTypeRatoon);
                Summary.WriteMessage(this, $"Applied {FertRatAmt} kg/ha of {FertTypeRatoon} fertilizer for ratoon.", MessageType.Information);
            }
        }
    }
}
