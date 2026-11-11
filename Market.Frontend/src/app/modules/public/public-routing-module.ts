import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { PublicLayoutComponent } from './public-layout/public-layout.component';
import { LandingComponent } from './landing/landing.component';

import { ConverterComponent } from './converter/converter.component';
import { myAuthData, myAuthGuard } from '../../core/guards/my-auth-guard';

const routes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    // children se prikazuju u <router-outlet> unutar PublicLayoutComponent
    children: [
      {
        path: '',
        component: LandingComponent
      },
      {
        // seedane promocije (prenesene iz V15) vode na /products?... → pretraga.
        // Query parametri ostaju (nepoznate pretraga ignoriše); nepoznate rute hvata '**' ispod.
        path: 'products',
        redirectTo: ''
      },
      {
        path: 'converter',
        component: ConverterComponent
      },
      // kasnije može i ovako:
      // { path: 'about', component: AboutComponent },
      // { path: 'contact', component: ContactComponent },

      { path: '**', redirectTo: '' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule {}
