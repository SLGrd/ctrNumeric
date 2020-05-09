using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace ctrNumeric
{
    public class ctrNumeric : System.Windows.Forms.TextBox
    {
        private int     m_DecimalPlaces = 0;
        private string  fmt = string.Empty;

        public int DecimalPlaces
        {
            get { return m_DecimalPlaces; }
            set
            {
                m_DecimalPlaces = value;

                //fmt = "{0:#,##0." + new string('0', DecimalPlaces) + "}";
                //this.Text = string.Format(fmt, value);
                //this.Select(Text.Length, 0);

                Invalidate();
            }
        }

        private int      _ValueInt { get; set; }
        private double   _ValueDbl { get; set; }
        private decimal  _ValueDcl { get; set; }

        public int ValueInt
        {
            get
            { return _ValueInt; }
            set
            {
                if (int.TryParse( value.ToString(), out value))
                {
                    _ValueInt = value;
                    _ValueDbl = value;
                    _ValueDcl = value;
                    fmt = "{0:#,##0." + new string('0', DecimalPlaces) + "}";
                    this.Text  = string.Format(fmt, value);
                    this.Select(this.Text.Length, 0);
                }
            }
        }

        public double ValueDbl
        {
            get
            { return _ValueDbl; }
            set
            { 
                if ( Double.TryParse(value.ToString(), out value))
                {
                    _ValueInt = Convert.ToInt32( value);
                    _ValueDbl = value;
                    _ValueDcl = Convert.ToDecimal( value);
                    fmt = "{0:#,##0." + new string('0', DecimalPlaces) + "}";
                    this.Text = string.Format(fmt, value);
                    this.Select(this.Text.Length, 0);
                }
            }
        }

        public decimal ValueDcl
        {
            get
            {   return _ValueDcl; }
            set
            {
                if ( Decimal.TryParse( value.ToString(), out value))
                {
                    _ValueInt = Convert.ToInt32(value);
                    _ValueDbl = Convert.ToDouble(value);
                    _ValueDcl = value;
                    fmt = "{0:#,##0." + new string('0', DecimalPlaces) + "}";
                    this.Text = string.Format(fmt, value);
                    this.Select(this.Text.Length, 0);
                }
            }
        }

        public ctrNumeric() { }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (char.IsDigit(e.KeyChar) || e.KeyChar.Equals((char)Keys.Back))
            {
                string w = Regex.Replace(this.Text, "[^0-9]", string.Empty);

                fmt = "{0:#,##0." + new string('0', DecimalPlaces) + "}";
                double dp = Math.Pow(10, DecimalPlaces);

                if (e.KeyChar.Equals((char)Keys.Back))              //  If backspace
                    w = ("00" + w).Substring(0, w.Length + 2 - 1);  //      takes out the rightmost digit
                else
                    w += e.KeyChar;

                _ValueInt = (int)(Double.Parse(w) / dp);
                _ValueDbl = (Double)(Double.Parse(w) / dp);
                _ValueDcl = (Decimal)(Double.Parse(w) / dp);

                //double d = (Double)(Double.Parse(w) / dp);

                this.Text = string.Format(fmt, Double.Parse(w) / dp);
                this.Select(this.Text.Length, 0);
            }
            e.Handled = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Delete)
            {
                _ValueInt = 0;
                _ValueDbl = 0;
                _ValueDcl = 0;
                //  Cast control
                string fmt = "{0:#,##0." + new string('0', DecimalPlaces) + "}";
                Text = string.Format(fmt, 0d);
                Select(Text.Length, 0);
                e.Handled = true;
            }
        }
    }
}
