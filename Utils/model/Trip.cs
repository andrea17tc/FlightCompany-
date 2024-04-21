using System;

namespace Utils.model
{
    [Serializable]
    public class Trip : Entity<int>
    {
        private Tourist tourist;
        private Purchase purchase;

        public Trip(Tourist tourist, Purchase purchase)
        {
            this.tourist = tourist;
            this.purchase = purchase;
        }

        public Tourist Tourist
        {
            get { return tourist; }
            set { tourist = value; }
        }

        public Purchase Purchase
        {
            get { return purchase; }
            set { purchase = value; }
        }
    
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Trip other = (Trip)obj;
            return tourist == other.tourist &&
                   purchase == other.purchase;
        }

        public override string ToString()
        {
            return tourist.ToString() + purchase.ToString() + '\n';
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}