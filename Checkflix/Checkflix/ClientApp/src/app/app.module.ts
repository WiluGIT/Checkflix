import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { MatTabsModule } from '@angular/material/tabs';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AdminComponent } from './admin/admin.component';
import { AuthorizeAdminGuard } from '../api-authorization/authorize.admin-guard';
import { ProductionFormComponent } from './production-form/production-form.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material';
import { MatSelectModule, MatSelect } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule, MatRippleModule, MAT_DATE_LOCALE } from '@angular/material/core';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatIconModule } from '@angular/material/icon';
import {MatPaginatorModule, MatPaginatorIntl} from '@angular/material/paginator';
import { PaginatorCustomComponent } from './paginator-custom/paginator-custom.component';
import { getPlPaginatorIntl } from './paginator-custom/pl-paginator-intl';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MultiSliderComponent } from './multi-slider/multi-slider.component';
import { ProductionComponent } from './production/production.component';
import { CollectionsComponent } from './collections/collections.component';
import { CollectionListComponent } from './collection-list/collection-list.component';
@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AdminComponent,
    ProductionFormComponent,
    PaginatorCustomComponent,
    MultiSliderComponent,
    ProductionComponent,
    CollectionsComponent,
    CollectionListComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'admin', component: AdminComponent, canActivate: [AuthorizeAdminGuard] },
      { path: 'production-form', component: ProductionFormComponent, canActivate: [AuthorizeAdminGuard] },
      { path: 'production-form/:id', component: ProductionFormComponent, canActivate: [AuthorizeAdminGuard] },
      { path: 'production/:id', component: ProductionComponent },
      { path: 'collections', component: CollectionsComponent, canActivate: [AuthorizeGuard]},
      { path: 'collections/favourites', component:CollectionListComponent, canActivate: [AuthorizeGuard]},
      { path: 'collections/to-watch', component:CollectionListComponent, canActivate: [AuthorizeGuard]},
      { path: 'collections/watched', component:CollectionListComponent, canActivate: [AuthorizeGuard]}
    ], { anchorScrolling: 'enabled'}),
    BrowserAnimationsModule,
    MatTabsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    FormsModule,
    ReactiveFormsModule,
    MatInputModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatTableModule,
    MatSortModule,
    MatIconModule,
    MatPaginatorModule,
    MatSnackBarModule,
    MatProgressSpinnerModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
    { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { appearance: 'fill' } },
    { provide: MAT_DATE_LOCALE, useValue: 'pl-Pl' },
    { provide: MatPaginatorIntl, useValue: getPlPaginatorIntl() }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
