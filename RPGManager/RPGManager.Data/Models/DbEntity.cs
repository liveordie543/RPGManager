namespace RPGManager.Data.Models
{
    public class DbEntity
    {
        public int Id { get; set; }

        public virtual bool IsAdded
        {
            get
            {
                return Id == 0;
            }
        }

        public bool IsModified { get; set; }

        public bool IsDeleted { get; set; }

        public virtual bool IsChanged
        {
            get
            {
                return IsAdded || IsModified || IsDeleted;
            }
        }
    }
}
