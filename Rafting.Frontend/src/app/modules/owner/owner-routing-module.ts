import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyProfileComponent } from '../account/my-profile/my-profile.component';
import { OwnerHomeComponent } from './owner-home/owner-home.component';
import { OwnerLayoutComponent } from './owner-layout/owner-layout.component';

const routes: Routes = [
  {
    path: '',
    component: OwnerLayoutComponent,
    children: [
      { path: '', component: OwnerHomeComponent },
      // ista stranica kao /admin/profile, deklarisana u AccountModule
      { path: 'profile', component: MyProfileComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OwnerRoutingModule {}