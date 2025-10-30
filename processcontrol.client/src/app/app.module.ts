import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ProcessListComponent } from './components/process-list/process-list.component';
import { ProcessModalComponent } from './components/process-modal/process-modal.component';
import { ProcessDetailComponent } from './components/process-detail/process-detail.component';
import { MovementFormComponent } from './components/movement-form/movement-form.component';

@NgModule({
  declarations: [
    AppComponent,
    ProcessListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    ProcessDetailComponent,
    MovementFormComponent,
    ProcessModalComponent
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
