using System;
using Models.Core;
using Models.PMF;
using APSIM.Shared.Utilities;
using Models.Surface;

//Tillage for long term simulation working!

namespace Models
{
    [Serializable]
    public class Script : Model
    {
        [Link] private Clock Clock;
        [Link] private SurfaceOrganicMatter SOM;
        [Link] private Sugarcane Sugarcane;

        [Description("First tillage operation date")]
        public string first_tillage_date { get; set; }

        [Description("First tillage depth (mm)")]
        public double first_tillage_depth { get; set; }

        [Description("First tillage Fraction of Residues To Incorporate (0-1)")]
        public double first_tillage_fraction { get; set; }

        [Description("Second tillage? yes=1")]
        public double second_tillage_test { get; set; }

        [Description("Second tillage depth (mm)")]
        public double second_tillage_depth { get; set; }

        [Description("Second tillage Fraction of Residues To Incorporate (0-1)")]
        public double second_tillage_fraction { get; set; }

        [Description("Third tillage? Yes=1")]
        public double third_tillage_test { get; set; }

        [Description("Third tillage depth (mm)")]
        public double third_tillage_depth { get; set; }

        [Description("Third tillage Fraction of Residues To Incorporate (0-1)")]
        public double third_tillage_fraction { get; set; }

        [Description("Fourth tillage? Yes=1")]
        public double fourth_tillage_test { get; set; }

        [Description("Fourth tillage depth (mm)")]
        public double fourth_tillage_depth { get; set; }

        [Description("Fourth tillage Fraction of Residues To Incorporate (0-1)")]
        public double fourth_tillage_fraction { get; set; }

        [Description("Number of days after end crop for subsequent tillages")]
        public int DaysAfterEndCrop { get; set; }

        private bool firstTillageDone = false;
        private bool secondTillageDone = false;
        private bool thirdTillageDone = false;
        private bool fourthTillageDone = false;
        private bool dfirstTillageDone= false;
        private int daysAfterEndCrop = 0; // Track days after end of crop
        private int simulationDays = 0; // Track total simulation days
        [Link] private ISummary Summary;

        [EventSubscribe("DoManagement")]
        private void OnDoManagement(object sender, EventArgs e)
        {
            simulationDays++; // Increment total simulation days

            // Perform first tillage operation on fixed date
            if (!firstTillageDone && DateUtilities.DatesEqual(first_tillage_date, Clock.Today))
            {
                SOM.Incorporate(first_tillage_fraction, first_tillage_depth);
                Summary.WriteMessage(this, $"First tillage operation performed on {first_tillage_date}.", MessageType.Information);
                firstTillageDone = true; // Mark as done
            }

            // Increment days after end crop if crop status is "out"
            if (Sugarcane.crop_status == "out")
            {
                daysAfterEndCrop++; // Increment days after end crop

                // Perform first tillage operation after 200 days only
                if (firstTillageDone && simulationDays >= 200 && daysAfterEndCrop >= DaysAfterEndCrop)
                {
                    // Only reapply if this is the first time after 200 days
                    if (!dfirstTillageDone) // Use a different flag if needed
                    {
                        SOM.Incorporate(first_tillage_fraction, first_tillage_depth);
                        Summary.WriteMessage(this, $"First tillage operation reapplied after {simulationDays} days and {daysAfterEndCrop} days.", MessageType.Information);
                        dfirstTillageDone = true; // Mark reapplication done
                        
                    }
                }

                // Perform subsequent tillages if first tillage has been done
                PerformTillageIfNeeded();
            }
            else
    {
        // Reset daysAfterEndCrop and tillage flags when crop is not "out"
        daysAfterEndCrop = 0;
        ResetTillageFlags();        
    }
        }

        private void PerformTillageIfNeeded()
        {
            // Second tillage operation
            if (second_tillage_test == 1 && !secondTillageDone && daysAfterEndCrop >= DaysAfterEndCrop)
            {
                SOM.Incorporate(second_tillage_fraction, second_tillage_depth);
                Summary.WriteMessage(this, "Second tillage operation performed.", MessageType.Information);
                secondTillageDone = true; // Mark as done
                return; // Exit to prevent further tillage
            }
            // Third tillage operation
            if (third_tillage_test == 1 && !thirdTillageDone && daysAfterEndCrop >= DaysAfterEndCrop)
            {
                SOM.Incorporate(third_tillage_fraction, third_tillage_depth);
                Summary.WriteMessage(this, "Third tillage operation performed.", MessageType.Information);
                thirdTillageDone = true; // Mark as done
                return; // Exit to prevent further tillage
            }

            // Fourth tillage operation
            if (fourth_tillage_test == 1 && !fourthTillageDone && daysAfterEndCrop >= DaysAfterEndCrop)
            {
                SOM.Incorporate(fourth_tillage_fraction, fourth_tillage_depth);
                Summary.WriteMessage(this, "Fourth tillage operation performed.", MessageType.Information);
                fourthTillageDone = true; // Mark as done
            }
            
        }
    
       private void ResetTillageFlags()
        {
            // Reset tillage flags if the crop is not "out"
            secondTillageDone = false;
            dfirstTillageDone= false;
            thirdTillageDone = false;
            fourthTillageDone = false;
           // Optionally keep firstTillageDone as is if you don't want to reset it
        }
    }
}
