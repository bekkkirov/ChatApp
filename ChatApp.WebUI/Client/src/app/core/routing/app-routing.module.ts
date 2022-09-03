import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import {SignInComponent} from "../components/sign-in/sign-in.component";
import {SignUpComponent} from "../components/sign-up/sign-up.component";
import {IsLoggedInGuard} from "../guards/is-logged-in.guard";
import {IsNotLoggedInGuard} from "../guards/is-not-logged-in.guard";

const routes: Routes = [
    {
        path: 'auth',
        runGuardsAndResolvers: "always",
        canActivate: [IsNotLoggedInGuard],
        children: [
            {path: 'sign-in', component: SignInComponent},
            {path: 'sign-up', component: SignUpComponent},
        ]
    },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
