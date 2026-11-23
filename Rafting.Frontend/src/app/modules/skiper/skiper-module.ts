import { NgModule } from '@angular/core';
import { AccountModule } from '../account/account-module';
import { SharedModule } from '../shared/shared-module';
import { SkiperHomeComponent } from './skiper-home/skiper-home.component';
import { SkiperLayoutComponent } from './skiper-layout/skiper-layout.component';
import { SkiperRoutingModule } from './skiper-routing-module';

@NgModule({
  declarations: [SkiperLayoutComponent, SkiperHomeComponent],
  imports: [
    SkiperRoutingModule,
    SharedModule,
    // "Moj profil" (/skiper/profile), ista stranica kao kod admina
    AccountModule
  ]
})
export class SkiperModule {}