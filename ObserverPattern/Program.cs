internal class Program
{
    private static void Main(string[] args)
    {
        Stock stock = new Stock();
        Bank bank = new Bank("СберБанк", stock);
        Broker broker = new Broker("Роман Валерьевич", stock);

        stock.Market();
        Console.WriteLine(new String('_', 28));

        stock.Market();
        Console.WriteLine(new String('_', 28));
        
        broker.StopTrade();
        Console.WriteLine("Брокер перестал вести торг");
        stock.Market();
        Console.WriteLine(new String('_', 28));


        Console.ReadKey();
    }
}

//interface ITarget
//{
//    void AddObserver(IObserver o);
//    void RemoveObserver(IObserver o);
//    void NotifyObservers();
//}

//class ConcreteTarget : ITarget
//{
//    private List<IObserver> observers;
//    public ConcreteTarget()
//    {
//        observers = new List<IObserver>();
//    }

//    public void AddObserver(IObserver o)
//    {
//        observers.Add(o);

//    }

//    public void RemoveObserver(IObserver o)
//    {
//        observers.Remove(o);
//    }

//    public void NotifyObservers()
//    {
//        foreach (IObserver observer in observers)
//            observer.Update();
//    }
//}


//interface IObserver
//{
//    void Update();
//}

//class ConcreteObserver : IObserver
//{
//    public void Update()
//    {

//    }
//}

interface IObserver
{
    void Update(Object ob);
}

interface ITarget //мне не нравится название "IObservable"
{
    void RegisterObserver(IObserver o);
    void RemoveObserver(IObserver o);
    void NotifyObservers();
}

class Stock : ITarget
{
    StockInfo stockInfo;
    List<IObserver> observers;

    public Stock()
    {
        observers = new List<IObserver>();
        stockInfo = new StockInfo();
    }
    
    public void RegisterObserver(IObserver o)
    {
        observers.Add(o);
    }

    public void RemoveObserver(IObserver o)
    {
        observers.Remove(o);
    }

    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(stockInfo);
        }
    }

    public void Market()
    {
        Random rnd = new Random();
        stockInfo.USD = rnd.Next(60, 90);
        stockInfo.EUR = rnd.Next(70, 100);
        NotifyObservers();
    }
}

class StockInfo
{
    public int USD { get; set; }
    public int EUR { get; set; }
}

class Broker : IObserver
{
    public string Name { get; set; }
    ITarget stock;

    public Broker(string name, ITarget target)
    {
        this.Name = name;
        stock = target;
        stock.RegisterObserver(this);
    }

    public void Update(Object ob)
    {
        StockInfo stockInfo = (StockInfo)ob;

        if (stockInfo.USD > 75)
            Console.WriteLine($"Брокер {this.Name} продает доллары; Курс доллара {stockInfo.USD}");
        else
            Console.WriteLine($"Брокер {this.Name} покупает доллары; Курс доллара {stockInfo.USD}");
    }

    public void StopTrade()
    {
        stock.RemoveObserver(this);
        stock = null;
    }
}

class Bank : IObserver
{
    public string Name { get; set; }
    ITarget stock;
    public Bank(string name, ITarget target)
    {
        this.Name=name;
        stock = target;
        stock.RegisterObserver(this);
    }
    public void Update(Object obj)
    {
        StockInfo stock = (StockInfo)obj;

        if(stock.EUR > 85)
            Console.WriteLine($"Банк {this.Name} продает евро; Курс евро: {stock.EUR}");
        else
            Console.WriteLine($"Банк {this.Name} покупает евро; Курс евро: {stock.EUR}");
    }
}