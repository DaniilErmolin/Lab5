namespace Domains.ViewModels
{
    public enum SortState
    {
        No, 
        NameAsc,    
        NameDesc,
        DescriptionAsc,
        DescriptionDesc


    }
    public class SortViewModel
    {
        public SortState NameSort { get; set; }

        public SortState DescriptionSort { get; set; }

        public SortState CurrentState { get; set; } 

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            DescriptionSort = sortOrder == SortState.DescriptionAsc ? SortState.DescriptionDesc : SortState.DescriptionAsc;

            CurrentState = sortOrder;
        }



    }
}