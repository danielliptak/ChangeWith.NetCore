import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { NgForm, NgModel } from '@angular/forms';

@Component({
    selector: 'home',
    template: `<h1>Exchanger</h1>

                <p *ngIf="!isLoaded"><em>Loading...</em></p>

                <form *ngIf="isLoaded" 
                        (ngSubmit)="startTransaction(baseOfExchange)" 
                        #exchangeForm="ngForm">

                    <div *ngIf="isLoaded" class="form-group">

                        <label for="name">Amount to change</label>
                        <input [(ngModel)] = "baseOfExchange.amount"
                                type="text" id="input"
                                required
                                name="amount"
                                #amount="ngModel"
                                >

                    </div> 

                    <div *ngIf="isLoaded" class="form-group">

                        <label for="exchangeSelect">From</label>
                        <select [(ngModel)] = "baseOfExchange.from"
                                class="form-control" id="currency"
                                required
                                name="currencyFrom"
                                #currencyFrom="ngModel"
                                >
                            <option value="" disabled selected>Select Currency</option>
                            <option *ngFor="let item of dates[0].cubeItems" 
                                [value]="item.currency"
                                >
                                {{item.currency}}
                            </option>
                        </select>

                    </div>

                    <div *ngIf="isLoaded" class="form-group">

                        <label for="exchangeSelect">To</label>
                        <select [(ngModel)] = "baseOfExchange.to"
                                class="form-control" id="date"
                                required
                                name="currencyTo"
                                #currencyTo="ngModel"
                                >
                            <option value="" disabled selected>Select Currency</option>
                            <option *ngFor="let item of dates[0].cubeItems" 
                                [value]="item.currency"
                                >
                                {{item.currency}}
                            </option>
                        </select>

                    </div>

                    <div *ngIf="isLoaded" class="form-group">

                        <label for="exchangeSelect">Select Date</label>
                        <select [(ngModel)] = "baseOfExchange.date"
                                class="form-control" id="date"
                                required
                                name="exchangeSelect"
                                #exchangeSelect="ngModel"
                                >
                            <option value="" disabled selected>Pick a Date</option>
                            <option *ngFor="let item of dates" 
                                [value]="item.time"
                                >
                                {{item.time}}
                            </option>
                        </select>

                        </div>

                        <button type="submit" class="btn btn-success" [disabled]="!exchangeForm.valid">Submit</button>

                </form>

                <div *ngIf="hasResult">
                    <h2>Result</h2>
                    <h3>{{baseOfExchange.to}}</h3>
                    <h3>{{changeResult.exchangeResult}}</h3>
                </div>
                
                <historyChart [rateHistory]=changeResult.rateHistory [dates]=timeLine>
                </historyChart>   
 
`
})


export class HomeComponent {

    private isLoaded: boolean = false;

    private hasResult: boolean = false;

    private timeLine: string[] = [];

    private changeResult: IChangeResult = {
        rateHistory: [],
        exchangeResult: 0
    }

    private baseOfExchange: IBaseOfExchange = {
        date: "",
        amount: "",
        from: "",
        to: "",
    };

    private dates: IDate[] = [];

    constructor(private http: Http, @Inject('BASE_URL') private baseUrl: string) {
        http.get(baseUrl + 'api/XML/GetCurrencies/all').subscribe(result => {
            this.dates = result.json();
            this.fillUpDates();
            this.isLoaded = true;
        }, error => console.error(error));
    }

    public startTransaction(dataInfoForExchange: IBaseOfExchange) {
        console.log(dataInfoForExchange);
        this.http.post(this.baseUrl + 'api/XML/Exchange', dataInfoForExchange).subscribe(result => {
            this.changeResult = result.json() as IChangeResult;
            this.hasResult = true;
        }, error => console.log(error));
    }

    private fillUpDates() {
        for (var item of this.dates) {
            this.timeLine.push(item.time);
        }
    }
}

interface IChangeResult {
    exchangeResult: number,
    rateHistory: string[],
}

interface IBaseOfExchange {
    date: string,
    amount: string,
    from: string,
    to: string,
}

interface IDate {
    id:string;
    time: string;
    cubeItems: ICubeItem[];
}

interface ICubeItem {
    id:string;
    rateStr: string;
    currency: string; 
}