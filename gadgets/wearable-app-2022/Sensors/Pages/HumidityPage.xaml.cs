﻿/*
 * Copyright (c) 2017 Samsung Electronics Co., Ltd All Rights Reserved
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using IktanRoving.Extensions;
using IktanRoving.Model;
using SkiaSharp;
using System;
using System.Collections.Generic;
using Tizen.Sensor;
using Tizen.Wearable.CircularUI.Forms;
using Xamarin.Forms.Xaml;

namespace IktanRoving.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HumidityPage : CirclePage
    {
        public HumidityPage()
        {
            Model = new HumidityModel
            {
                IsSupported = HumiditySensor.IsSupported,
                SensorCount = HumiditySensor.Count
            };

            InitializeComponent();

            if (Model.IsSupported)
            {
                Humidity = new HumiditySensor();
                Humidity.DataUpdated += Humidity_DataUpdated;

                canvas.Series = new List<Series>()
                {
                    new Series()
                    {
                        Color = SKColors.Red,
                        Name = "Humidity",
                        FormattedText = "Humidity={0}",
                    },
                };
            }
        }

        public HumiditySensor Humidity { get; private set; }

        public HumidityModel Model { get; private set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Humidity?.Start();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Humidity?.Stop();
        }

        private void Humidity_DataUpdated(object sender, HumiditySensorDataUpdatedEventArgs e)
        {
            Model.Humidity = e.Humidity;
            canvas.Series[0].Points.Add(new Extensions.Point() { Ticks = DateTime.UtcNow.Ticks, Value = e.Humidity });
            canvas.InvalidateSurface();
        }
    }
}