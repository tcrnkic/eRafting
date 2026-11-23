import { NgModule } from '@angular/core';
import { AccountModule } from '../account/account-module';
import { SharedModule } from '../shared/shared-module';
import { OwnerHomeComponent } from './owner-home/owner-home.component';
import { OwnerLayoutComponent } from './owner-layout/owner-layout.component';
import { OwnerRoutingModule } from './owner-routing-module';

@NgModule({
  declarations: [OwnerLayoutComponent, OwnerHomeComponent],
  imports: [
    OwnerRoutingModule,
    SharedModule,
    // "Moj profil" (/owner/profile), ista stranica kao kod admina
    AccountModule
  ]
})
export class OwnerModule {}