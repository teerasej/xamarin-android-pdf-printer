﻿using System;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Print;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Xamarin.Essentials;

namespace PDFPrinter
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button buttonDownloadPDF;
        Button buttonViewPDF;
        Button buttonPrintPDF;

        Uri localPDFFile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            buttonDownloadPDF = FindViewById<Button>(Resource.Id.buttonDownloadPDF);
            buttonViewPDF = FindViewById<Button>(Resource.Id.buttonViewPDF);
            buttonPrintPDF = FindViewById<Button>(Resource.Id.buttonPrintPDF);

            buttonViewPDF.Enabled = false;
            buttonPrintPDF.Enabled = false;

            buttonDownloadPDF.Click += async (sender, e) =>
            {
                var targetFileUrl = "https://www.nextflow.in.th/demo/xamarin-android/files/flutter.pdf";

                var localAppDataFolderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
                var downloader = new DownloadService(localAppDataFolderPath);

                localPDFFile = await downloader.Download(targetFileUrl);

                buttonViewPDF.Enabled = true;
                buttonPrintPDF.Enabled = true;
            };

            buttonViewPDF.Click += (sender, e) =>
            {
                var intent = new Intent(Application.Context, typeof(PDFViewerActivity));
                intent.PutExtra("path", localPDFFile.AbsolutePath);

                StartActivity(intent);
            };

            buttonPrintPDF.Click += async (sender, e) =>
            {
                await Share.RequestAsync(new ShareFileRequest
                {
                    Title = "PDF File",
                    File = new ShareFile(localPDFFile.AbsolutePath, "application/pdf")
                });

                //var printManager = (PrintManager)GetSystemService(Context.PrintService);
                //var printAdapter = new CustomPrintDocumentAdapter(localPDFFile.AbsolutePath);
                //printManager.Print("My print", printAdapter, null);
            };

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}