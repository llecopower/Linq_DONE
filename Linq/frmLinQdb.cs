using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace Linq
{
    public partial class frmLinQdb : Form
    {
        public frmLinQdb()
        {
            InitializeComponent();
        }

        //We used dataset to store many DataTables in one single collection.
        DataSet myset = new DataSet();
        private void frmLinQdb_Load(object sender, EventArgs e)
        {
            OleDbConnection myCon = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\alex\source\repos\Linq\Linq\bin\Debug\Data\School.mdb");
            myCon.Open();

            OleDbCommand mycmd = new OleDbCommand("SELECT * FROM Courses",myCon);
            OleDbDataAdapter adpCourses = new OleDbDataAdapter(mycmd);
            adpCourses.Fill(myset, "Courses");

            OleDbCommand mycmd2 = new OleDbCommand("SELECT * FROM Students", myCon);
            OleDbDataAdapter adpStudents = new OleDbDataAdapter(mycmd2);
            adpStudents.Fill(myset, "Students");


            GridResult.DataSource = myset.Tables["Students"];

             //GridResult.DataSource = myset.Tables["Courses"];

            //Fill the combobox with LINQ

            var CourseTR = from DataRow cr in myset.Tables["Courses"].Rows
                           select new
                           {
                               Titles = cr.Field<string>("Title"),
                               refC = cr.Field<Int32>("RefCourse")
                           };

            cboCourse.DisplayMember = "Titles";
            cboCourse.ValueMember = "refC";

            cboCourse.DataSource = CourseTR.ToList();






        }

        private void cboCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
           int refC = Convert.ToInt32(cboCourse.SelectedValue.ToString());

            var student = from DataRow st in myset.Tables["Students"].Rows
                          where st.Field<Int32>("RefCourse") == refC
                          select st;

            GridResult.DataSource = student.CopyToDataTable();

        }
    }
}
