using System;
using System.Collections.Generic;
using System.Threading;


//CREADO POR ZAMBRANO ANCHUNDIA JULIO CESAR

namespace obserbador
{
    public interface IObserver
    {
        // Recibir actualización del asunto
        void Update(ISubject subject);
    }

    public interface ISubject
    {
        // Adjunte un observador al sujeto.
        void Attach(IObserver observer);

        // Separe a un observador del sujeto.
        void Detach(IObserver observer);

        // Notificar a todos los observadores sobre un evento.
        void Notify();
    }

    // El Sujeto posee algún estado importante y notifica a los observadores cuando el estado cambia.
    public class Subject : ISubject
    {
        // En la simplicidad, el estado del Asunto, esencial para todos los suscriptores, se almacena en esta variable.
        public int State { get; set; } = -0;

        // Lista de suscriptores. En la vida real, 
        //puede almacenar de manera más completa (clasificada por tipo de evento, etc.).

        private List<IObserver> _observers = new List<IObserver>();

        // Los métodos de gestión de suscripciones.
        public void Attach(IObserver observer)
        {
            Console.WriteLine("Asunto: Adjunto un observador.");
            this._observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            this._observers.Remove(observer);
            Console.WriteLine("Asunto: Separado un observador.");
        }

        // Activar una actualización en cada suscriptor.
        public void Notify()
        {
            Console.WriteLine("Notificador: Notificando a los observadores ...");

            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }

        /* Por lo general, la lógica de suscripción es solo una fracción de lo que realmente 
        puede hacer un Sujeto. Los sujetos suelen tener una lógica comercial importante, que activa 
        un método de notificación cada vez que algo importante está a punto de suceder (o después).*/

        public void SomeBusinessLogic()
        {
            Console.WriteLine("\nSujeto: Estoy haciendo algo importante.");
            this.State = new Random().Next(0, 10);

            Thread.Sleep(15);

            Console.WriteLine("Sujeto: Mi estado acaba de cambiar a:" + this.State);
            this.Notify();
        }
    }

    // Los Observadores Concretos reaccionan a las actualizaciones emitidas por el Sujeto al que se han adherido.
    class ConcreteObserverA : IObserver
    {
        public void Update(ISubject subject)
        {            
            if ((subject as Subject).State < 3)
            {
                Console.WriteLine("Observador A: Reaccionó al evento.");
            }
        }
    }

    class ConcreteObserverB : IObserver
    {
        public void Update(ISubject subject)
        {
            if ((subject as Subject).State == 0 || (subject as Subject).State >= 2)
            {
                Console.WriteLine("Observador B: Reaccionó al evento.");
            }
        }
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            // El codigo del cliente.
            var subject = new Subject();
            var observerA = new ConcreteObserverA();
            subject.Attach(observerA);

            var observerB = new ConcreteObserverB();
            subject.Attach(observerB);

            subject.SomeBusinessLogic();
            subject.SomeBusinessLogic();

            subject.Detach(observerB);

            subject.SomeBusinessLogic();
        }
    }
}