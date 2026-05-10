using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task2
{
    interface IMediator
    {
        bool RequestLanding(Aircraft aircraft);
        void RequestTakeOff(Aircraft aircraft);
    }

    class CommandCentre : IMediator
    {
        private List<Runway> _runways = new List<Runway>();
        private List<Aircraft> _aircrafts = new List<Aircraft>();

        public void RegisterRunway(Runway runway) => _runways.Add(runway);
        public void RegisterAircraft(Aircraft aircraft) => _aircrafts.Add(aircraft);

        public bool RequestLanding(Aircraft aircraft)
        {
            Console.WriteLine($"[CommandCentre]: Checking available runways for {aircraft.Name}...");

            var freeRunway = _runways.FirstOrDefault(r => r.IsBusyWithAircraft == null);

            if (freeRunway != null)
            {
                Console.WriteLine($"[CommandCentre]: Runway {freeRunway.Id} is clear. Permission granted.");
                freeRunway.IsBusyWithAircraft = aircraft;
                freeRunway.HighLightRed();
                aircraft.CurrentRunwayId = freeRunway.Id;
                return true;
            }

            Console.WriteLine($"[CommandCentre]: Access denied. All runways are busy.");
            return false;
        }

        public void RequestTakeOff(Aircraft aircraft)
        {
            var runway = _runways.FirstOrDefault(r => r.IsBusyWithAircraft == aircraft);
            if (runway != null)
            {
                Console.WriteLine($"[CommandCentre]: {aircraft.Name} is taking off from {runway.Id}.");
                runway.IsBusyWithAircraft = null;
                runway.HighLightGreen();
                aircraft.CurrentRunwayId = null;
            }
        }
    }

    class Aircraft
    {
        public string Name { get; }
        public Guid? CurrentRunwayId { get; set; }
        private IMediator _mediator;

        public Aircraft(string name, IMediator mediator)
        {
            Name = name;
            _mediator = mediator;
        }

        public void Land()
        {
            Console.WriteLine($"Aircraft {Name}: Requesting landing...");
            if (_mediator.RequestLanding(this))
            {
                Console.WriteLine($"Aircraft {Name}: Landed successfully.");
            }
        }

        public void TakeOff()
        {
            if (CurrentRunwayId != null)
            {
                Console.WriteLine($"Aircraft {Name}: Requesting takeoff...");
                _mediator.RequestTakeOff(this);
            }
        }
    }

    class Runway
    {
        public readonly Guid Id = Guid.NewGuid();
        public object IsBusyWithAircraft { get; set; }

        public void HighLightRed() => Console.WriteLine($"Runway {Id} Light: RED (Busy)");
        public void HighLightGreen() => Console.WriteLine($"Runway {Id} Light: GREEN (Free)");
    }

    class Program
    {
        static void Main()
        {
            var center = new CommandCentre();

            var runway1 = new Runway();
            var runway2 = new Runway();
            center.RegisterRunway(runway1);
            center.RegisterRunway(runway2);

            var boeing = new Aircraft("Boeing 747", center);
            var airbus = new Aircraft("Airbus A320", center);
            var an = new Aircraft("Antonov An-225", center);

            boeing.Land();
            airbus.Land();
            an.Land();

            Console.WriteLine("\nProcessing Takeoffs");
            boeing.TakeOff();

            an.Land();
        }
    }
}
