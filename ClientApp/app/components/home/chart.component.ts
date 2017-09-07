import { Component, Input, AfterViewInit } from '@angular/core';
//import * as Plotly from 'plotly.js/lib/core';
import { PlotlyHTMLElement, newPlot } from 'plotly.js';

//declare function require(s: string): any;
//const plotly: any = require('plotly.js/dist/plotly-basic.min');

@Component({
    selector: 'historyChart',
    template: `<h1>chart</h1>
                <div id="historyChart">
                       
                </div>
`
})

export class ChartComponent implements AfterViewInit {

    @Input()
    rateHistory: any[] = [];

    @Input()
    dates: any[] = [];



    private dataTraces: any[] = [];
    private divId: string = 'historyChart';
    private plotElement: any;
    private trace2: any = {
        type: "scatter",
        mode: "lines",
        name: 'AAPL Low',
        x: this.dates,
        y: this.rateHistory,
        line: { color: '#7F7F7F' }
    }
    constructor() {

    }

    ngAfterViewInit() {
        this.dataTraces.push(this.trace2);
        //this.plotElement = Plotly.newPlot(this.divId, this.dataTraces);
    }

    //var trace1 = {
    //    type: "scatter",
    //    mode: "lines",
    //    name: 'AAPL High',
    //    x: unpack(rows, 'Date'),
    //    y: unpack(rows, 'AAPL.High'),
    //    line: { color: '#17BECF' }
    //}

    //var layout = {
    //    title: 'Basic Time Series',
    //};

}