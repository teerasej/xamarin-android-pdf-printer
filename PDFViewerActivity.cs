
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.App;
using Syncfusion.SfPdfViewer.Android;

namespace PDFPrinter
{
    [Activity(Label = "Preview", ParentActivity = typeof(MainActivity))]
    public class PDFViewerActivity : AppCompatActivity
    {
        SfPdfViewer pdfViewer;
        Stream stream;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            // Create your application here
            SetContentView(Resource.Layout.activity_viewer);
            pdfViewer = FindViewById<SfPdfViewer>(Resource.Id.pdfViewer);


            var pdfFilePath = Intent.GetStringExtra("path");

            stream = File.Open(pdfFilePath, FileMode.Open);
            pdfViewer.LoadDocument(stream);



        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Finish();
            return true;
        }

        public override void OnBackPressed()
        {
            Finish();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            stream.Dispose();
        }

    } 
}
