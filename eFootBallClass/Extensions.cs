using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace eFootBallClass
{
    public static class Extensions
    {
        public static int? ToInt(this object obj)
        {
            if (obj == null) { return null; }
            try
            {
                return int.Parse(obj.ToString());
            }
            catch { return null; }
        }

        public static long? ToLong(this object obj)
        {
            if (obj == null) { return null; }
            try
            {
                return long.Parse(obj.ToString());
            }
            catch { return null; }
        }

        public static double? ToDouble(this object obj)
        {
            if (obj == null) { return null; }
            try
            {
                return double.Parse(obj.ToString());
            }
            catch { return null; }
        }

        public static bool? ToBool(this object obj)
        {
            if (obj == null) { return null; }
            try
            {
                return bool.Parse(obj.ToString());
            }
            catch { return null; }
        }

        public static DateTime? ToDateTime(this object obj)
        {
            if (obj == null) { return null; }
            try
            {
                return DateTime.Parse(obj.ToString());
            }
            catch { return null; }
        }

        public static int Update(this DataTable dt, SqlDataAdapter da)
        {
            using (SqlCommandBuilder cmb = new SqlCommandBuilder(da))
            {
                int kq = da.Update(dt);
                dt.AcceptChanges();
                return kq;
            }
        }

        public static object ToSqlParam(this object obj)
        {
            if (obj == null) { return (object)DBNull.Value; }

            //if (obj.GetType() == typeof(DateTime))
            //{
            //    return ((DateTime)obj).AddHours(7);
            //}
            return obj;
        }


        public static T ToObject<T>(this string inputStr)
        {
            return JsonConvert.DeserializeObject<T>(inputStr);
        }

        public static T ToObject<T>(this DataRow dr)
        {
            if (dr == null) { return default(T); }
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            if (dr[column.ColumnName] == DBNull.Value)
                            {
                                pro.SetValue(obj, null, null);
                            }
                            else
                            {
                                if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int?))
                                {
                                    pro.SetValue(obj, dr[column.ColumnName].ToInt(), null);
                                }
                                else if (pro.PropertyType == typeof(long) || pro.PropertyType == typeof(long?))
                                {
                                    pro.SetValue(obj, dr[column.ColumnName].ToLong(), null);
                                }
                                else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
                                {
                                    pro.SetValue(obj, dr[column.ColumnName].ToDateTime(), null);
                                }
                                else
                                {
                                    pro.SetValue(obj, dr[column.ColumnName], null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(pro.Name + ": " + ex.Message);
                        }
                    }
                }
            }
            return obj;
        }

        public static T ToObject<T>(this DataRowView dr)
        {
            if (dr == null) { return default(T); }
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.DataView.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                    {
                        try
                        {
                            if (dr.Row[column.ColumnName] == DBNull.Value)
                            {
                                pro.SetValue(obj, null, null);
                            }
                            else
                            {
                                if (pro.PropertyType == typeof(int) || pro.PropertyType == typeof(int?))
                                {
                                    pro.SetValue(obj, dr.Row[column.ColumnName].ToInt(), null);
                                }
                                else if (pro.PropertyType == typeof(long) || pro.PropertyType == typeof(long?))
                                {
                                    pro.SetValue(obj, dr.Row[column.ColumnName].ToLong(), null);
                                }
                                else if (pro.PropertyType == typeof(DateTime) || pro.PropertyType == typeof(DateTime?))
                                {
                                    pro.SetValue(obj, dr.Row[column.ColumnName].ToDateTime(), null);
                                }
                                else
                                {
                                    pro.SetValue(obj, dr.Row[column.ColumnName], null);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(pro.Name + ": " + ex.Message);
                        }
                    }
                }
            }
            return obj;
        }

        public static ObservableCollection<T> ToListObject<T>(this DataTable dt)
        {
            ObservableCollection<T> lstObject = new ObservableCollection<T>();

            foreach (DataRow dr in dt.Rows)
            {
                lstObject.Add(ToObject<T>(dr));
            }
            return lstObject;
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public RelayCommand(Predicate<T> canExecute, Action<T> execute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }
            _canExecute = canExecute;
            _execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            try
            {
                return _canExecute == null ? true : _canExecute((T)parameter);
            }
            catch
            {
                return true;
            }

        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
