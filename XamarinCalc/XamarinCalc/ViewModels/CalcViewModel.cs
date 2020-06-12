using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinCalc.Models;



namespace XamarinCalc.ViewModels
{
    public class CalcViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public CalcData CalcData { get; private set; }

        private Func<double, double, double> Operation;

        private bool? Flag = null;

        private bool LastWasSign = false;

        private bool WasDot = false;

        private string history;

        private string summ;


        public ICommand SetNumberCommand { protected set; get; }


        public ICommand AddCommand { protected set; get; }

        public ICommand SubCommand { protected set; get; }

        public ICommand MultCommand { protected set; get; }
        public ICommand DevCommand { protected set; get; }

        public ICommand CalculateCommand { protected set; get; }

        public ICommand DotCommand { protected set; get; }

        public ICommand ClearCommand { protected set; get; } 

        public CalcViewModel()
        {
            CalcData = new CalcData();

            CalcData.Result = "0";

            SetNumberCommand = new Command(SetNumber);

            AddCommand = new Command(Add);
            SubCommand = new Command(Sub);
            MultCommand = new Command(Mult);
            DevCommand = new Command(Dev);
            CalculateCommand = new Command(Calculate);

            DotCommand = new Command(Dot);

            ClearCommand = new Command(Clear);
        }

        public string Number1
        {
            get { return CalcData.Number1; }
            set
            {
                if (CalcData.Number1 != value)
                {
                    CalcData.Number1 = value;
                    OnPropertyChanged("Number1");
                }
            }
        }

        public string Number2
        {
            get { return CalcData.Number2; }
            set
            {
                if (CalcData.Number2 != value)
                {
                    CalcData.Number2 = value;
                    OnPropertyChanged("Number2");
                }
            }
        }

        public string Result
        {
            get { return CalcData.Result; }
            set
            {
                if (CalcData.Result != value)
                {
                    CalcData.Result = value;
                    OnPropertyChanged("Result");
                }
            }
        }

        public string History
        {
            get { return history; }
            set
            {
                if (history != value)
                {
                    history = value;
                    OnPropertyChanged("History");
                }
            }
        }

        public string Summ
        {
            get { return summ; }
            set
            {
                if (summ != value)
                {
                    summ = value;
                    OnPropertyChanged("Summ");
                }
            }
        }

        public void Calculate() 
        {
            try
            {
                if (Number1 != null && Number2 != null)
                {
                    double number1 = this.ConvertToDouble(Number1);
                    double number2 = this.ConvertToDouble(Number2);
                    // Here Android returns "," instead "."
                    Result = Operation(number1, number2).ToString().Replace(",",".");
                    Summ = Result;

                    Number1 = Result;
                    Number2 = null;
                    Operation = null;
                    Flag = true;
                    LastWasSign = false;
                }
            }
            catch(Exception ex) 
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void SetNumber(object sender)
        {
            var btn = sender as Xamarin.Forms.Button;
            string tmp = btn.Text;
            if (Flag == null)
            {
                Flag = false;
                Number1 = tmp;
                Result = tmp;
                History = tmp;
            }
            else if (Flag == false)
            {
                Number1 += tmp;
                Result += tmp;
                History += tmp;
            }
            else if(Flag == true) 
            {
                Number2 += tmp;
                Result += tmp;
                History += tmp;
            }
        }

        private void Add()
        {
            if (!LastWasSign)
            {
                Flag = true;
                LastWasSign = true;
                WasDot = false;
                Result += "+";
                History += "+";
                Operation = delegate (double a, double b)
                { return Math.Round(a + b, 2 ); };
            }
        }

        private void Sub()
        {
            if (string.Compare("0", Result) == 0)
            {
                Flag = false;
                Result = "-";
                History = "-";
                Number1 = "-";
            }
            if (!LastWasSign && (string.Compare("-", Result) != 0))
            {
                Flag = true;
                LastWasSign = true;
                WasDot = false;
                Result += "-";
                History += "-";
                Operation = delegate (double a, double b)
                { return Math.Round(a - b, 2); };
            }
        }

        private void Mult()
        {
            if (!LastWasSign)
            {
                Flag = true;
                LastWasSign = true;
                WasDot = false;
                Result += "*";
                History += "*";
                Operation = delegate (double a, double b)
                { return Math.Round(a * b, 2); };
            }
        }

        private void Dev()
        {
            if (!LastWasSign)
            {
                Flag = true;
                LastWasSign = true;
                Result += "/";
                History += "/";
                WasDot = false;
                Operation = delegate (double a, double b)
                { return Math.Round(a / b, 2); };
            }
        }

        private void Dot()
        {
            if ((Result != string.Empty || Result != null) && !WasDot)
            {
                WasDot = true;
                Result += ".";
                History += ".";

                if (Flag == false)
                {
                    Number1 += ".";
                }
                if (Flag == true)
                {
                    Number2 += ".";
                }
            }
        }

        private void Clear()
        {
            Number1 = null;
            Number2 = null;
            Result = string.Empty;
            Operation = null;
            Flag = null;
            LastWasSign = false;
            WasDot = false;
            History = string.Empty;
            Summ = string.Empty;
    }

        private double ConvertToDouble(string number)
        {
            try 
            {
                double result = double.Parse(number, System.Globalization.CultureInfo.InvariantCulture);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        protected void OnPropertyChanged(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
