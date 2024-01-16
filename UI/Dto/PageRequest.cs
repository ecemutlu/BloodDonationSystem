namespace UI.Dto
{
    public class PageRequest
    {      
        public int Page { get; set; }
        public int PageSize { get; set; }

        public PageRequest()
        {

        }

        public PageRequest(int page, int pageSize)
        {
            Page = page;
            PageSize = pageSize;
        }
        
    }
}
