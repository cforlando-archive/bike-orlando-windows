using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace BikeOrlando.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged
    {

        public ViewModelBase()
        {

        }



        protected virtual void RaisePropertyChanged(Expression<Func<object>> expression)
        {
            if (PropertyChanged != null)
            {
                var body = expression.Body as MemberExpression;
                string propertyName = body.Member.Name;
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

    }
}
