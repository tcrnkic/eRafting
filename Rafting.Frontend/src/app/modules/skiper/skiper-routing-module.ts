import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyProfileComponent } from '../account/my-profile/my-profile.component';
import { SkiperHomeComponent } from './skiper-home/skiper-home.component';
import { SkiperLayoutComponent } from './skiper-layout/skiper-layout.component';

const routes: Routes = [
  {
    path: '',
    component: SkiperLayoutComponent,
    children: [
      { path: '', component: SkiperHomeComponent },
      // ista stranica kao /admin/profile, deklarisana u AccountModule
      { path: 'profile', component: MyProfileComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SkiperRoutingModule {}