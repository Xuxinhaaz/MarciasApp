
import { Component, inject } from '@angular/core';
import { HeaderComponent } from "../../../Components/header/header.component";
import { Router, RouterLink, RouterModule } from '@angular/router';
import {
  ReactiveFormsModule,
  FormBuilder,
  FormGroup,
  Validators,
} from '@angular/forms';
import { environment } from '../../../../environments/environment.development';
import { NgIf } from '@angular/common';
import { SupabaseAuthClient } from '@supabase/supabase-js/dist/module/lib/SupabaseAuthClient';
import { SupabaseService } from '../../../services/supabase.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [HeaderComponent, ReactiveFormsModule, NgIf, RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  registerForm!: FormGroup;

  constructor(private formBuilder: FormBuilder, private auth: SupabaseService, private router: Router) {
    this.registerForm = this.formBuilder.group({
      email: formBuilder.control('', [Validators.required, Validators.email]),
      password: formBuilder.control('', [Validators.required, Validators.minLength(7)]),
    });
  }

  public onSubmit(){
    this.auth.signUp(this.registerForm.value.email, this.registerForm.value.password).then(async (res) =>{
      console.log(res);
      if(res.data.user!.role === "authenticated"){
        await this.router.navigate(['/market']);
      }
    }).catch((err) =>{console.log(err)});
  }
 }
