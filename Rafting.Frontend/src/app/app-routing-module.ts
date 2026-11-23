import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {myAuthData, myAuthGuard} from './core/guards/my-auth-guard';

const routes: Routes = [
  {
    path: 'admin',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireAuth: true, requireAdmin: true }),
    loadChildren: () =>
      import('./modules/admin/admin-module').then(m => m.AdminModule)
  },
  {
    path: 'owner',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireManager: true }),
    loadChildren: () =>
      import('./modules/owner/owner-module').then(m => m.OwnerModule)
  },
    {
    path: 'skiper',
    canActivate: [myAuthGuard],
    data: myAuthData({ requireEmployee: true }),
    loadChildren: () =>
      import('./modules/skiper/skiper-module').then(m => m.SkiperModule)
  },
  {
    path: 'auth',
    loadChildren: () =>
      import('./modules/auth/auth-module').then(m => m.AuthModule)
  },
  {
    path: '',
    loadChildren: () =>
      import('./modules/public/public-module').then(m => m.PublicModule)
  },
  // fallback 404
  { path: '**', redirectTo: '' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
