using System;
using Utils.model;

namespace Utils.model
{
    [Serializable]
    public class Tourist : Entity<int>
    {
        private string touristName;
    
        public Tourist(string touristName) {
            this.touristName = touristName;
        }

        public string TouristName
        {
            get { return touristName;  }
            set { touristName = value; }
        }
    
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Tourist other = (Tourist)obj;
            return touristName == other.touristName;
        }

        public override string ToString()
        {
            return touristName+ '\n';
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}