import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProcessListComponent } from './components/process-list/process-list.component';
import { ProcessDetailComponent } from './components/process-detail/process-detail.component';

const routes: Routes = [
  { path: '', redirectTo: '/processes', pathMatch: 'full' },
  { path: 'processes', component: ProcessListComponent },
  { path: 'processes/:id', component: ProcessDetailComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
