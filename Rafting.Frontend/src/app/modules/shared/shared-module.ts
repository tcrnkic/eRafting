import { NgModule } from '@angular/core';
import { DashboardShellComponent } from './components/dashboard-shell/dashboard-shell.component';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import {FitPaginatorBarComponent} from './components/fit-paginator-bar/fit-paginator-bar.component';
import {materialModules} from './material-modules';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {TranslatePipe} from '@ngx-translate/core';
import { FitConfirmDialogComponent } from './components/fit-confirm-dialog/fit-confirm-dialog.component';
import {DialogHelperService} from './services/dialog-helper.service';
import { FitLoadingBarComponent } from './components/fit-loading-bar/fit-loading-bar.component';
import { FitTableSkeletonComponent } from './components/fit-table-skeleton/fit-table-skeleton.component';
import { FitFieldErrorComponent } from './components/fit-field-error/fit-field-error.component';
import { CurrencySelectorComponent } from './components/currency-selector/currency-selector.component';
import { StoreHeaderComponent } from './components/store-header/store-header.component';
import { MoneyPipe } from './pipes/money.pipe';
import { ApiUrlPipe } from './pipes/api-url.pipe';
import { CardComponent } from './components/card/card.component';
import { EmptyStateComponent } from './components/empty-state/empty-state.component';



@NgModule({
  declarations: [

    FitPaginatorBarComponent,
    FitConfirmDialogComponent,
    FitLoadingBarComponent,
    FitTableSkeletonComponent,
    FitFieldErrorComponent,
    CurrencySelectorComponent,
    StoreHeaderComponent,
    MoneyPipe,
        DashboardShellComponent,
    CardComponent,
    EmptyStateComponent,
    ApiUrlPipe
  ],
  imports: [
    CommonModule,
    // routerLink u StoreHeaderComponent (samo direktive, bez ruta)
    RouterModule,
    ReactiveFormsModule,
    FormsModule,
    TranslatePipe,
    ...materialModules
  ],
  providers: [
    DialogHelperService
  ],
  exports:[
  
    FitPaginatorBarComponent,
    CommonModule,
    ReactiveFormsModule,
    TranslatePipe,
    FormsModule,
    FitLoadingBarComponent,
    FitTableSkeletonComponent,
    FitFieldErrorComponent,
    CurrencySelectorComponent,
    StoreHeaderComponent,
    MoneyPipe,
    ApiUrlPipe,
      DashboardShellComponent,
    CardComponent,
    EmptyStateComponent,
    materialModules
  ]
})
export class SharedModule { }
