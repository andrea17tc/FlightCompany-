using System;

namespace Utils.model
{
    [Serializable]
    public class Flight : Entity<int>
    { 
        private string destination;
        private DateTime date;
        private string airport;
        private int noTotalSeats;

        public Flight(string destination, DateTime date, string airport, int noSeats)
        {
            this.destination = destination;
            this.date = date;
            this.airport = airport;
            this.noTotalSeats = noSeats;
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Airport
        {
            get { return airport; }
            set { airport = value; }
        }

        public int NoTotalSeats
        {
            get { return noTotalSeats; }
            set { noTotalSeats = value; }
        }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (obj == null || GetType() != obj.GetType())
                return false;

            Flight flight = (Flight)obj;
            return noTotalSeats == flight.noTotalSeats &&
                   destination == flight.destination &&
                   date == flight.date &&
                   airport == flight.airport;
        }

        public override string ToString()
        {
            return $"Flight to {destination} at {date} from {airport}. Number of Seats={noTotalSeats}";
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
