import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { library } from '@fortawesome/fontawesome-svg-core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import {
  faUserPlus, faCoffee, faTimes, faInfoCircle, faUsers,
  faCheck, faTimesCircle, faQuestionCircle, faCheckCircle,
  faEdit, faPlusCircle, faHome, faCogs, faCubes, faTint,
  faUser, faClone, faBox, faBook, faObjectGroup, faDotCircle, faSignOutAlt
} from '@fortawesome/free-solid-svg-icons';

import {
  MatAutocompleteModule, MatBadgeModule, MatBottomSheetModule, MatButtonModule,
  MatButtonToggleModule, MatCardModule, MatCheckboxModule, MatChipsModule,
  MatDatepickerModule, MatDialogModule, MatDividerModule, MatExpansionModule,
  MatGridListModule, MatIconModule, MatInputModule, MatListModule, MatMenuModule,
  MatNativeDateModule, MatPaginatorModule, MatProgressBarModule, MatProgressSpinnerModule,
  MatRadioModule, MatRippleModule, MatSelectModule, MatSidenavModule, MatSliderModule,
  MatSlideToggleModule, MatSnackBarModule, MatSortModule, MatStepperModule,
  MatTableModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
  MatTreeModule
} from '@angular/material';

import { CdkTableModule } from '@angular/cdk/table';
import { CdkTreeModule } from '@angular/cdk/tree';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FlexLayoutModule } from '@angular/flex-layout';

library.add(
  faUserPlus, faCoffee, faTimes, faTimesCircle,
  faInfoCircle, faQuestionCircle, faCheckCircle,
  faUsers, faCheck, faEdit, faPlusCircle, faHome,
  faCogs, faCubes, faTint, faUser, faClone,
  faObjectGroup, faDotCircle, faBox, faBook, faSignOutAlt
);


@NgModule({
  imports: [
    FontAwesomeModule, FlexLayoutModule, FormsModule, ReactiveFormsModule,
    CommonModule, RouterModule, MatAutocompleteModule, MatBadgeModule,
    MatBottomSheetModule, MatButtonModule, MatButtonToggleModule, MatCardModule,
    MatCheckboxModule, MatChipsModule, MatDatepickerModule, MatDialogModule,
    MatDividerModule, MatExpansionModule, MatGridListModule, MatIconModule,
    MatInputModule, MatListModule, MatMenuModule, MatNativeDateModule,
    MatPaginatorModule, MatProgressBarModule, MatProgressSpinnerModule, MatRadioModule,
    MatRippleModule, MatSelectModule, MatSidenavModule, MatSliderModule,
    MatSlideToggleModule, MatSnackBarModule, MatSortModule, MatStepperModule,
    MatTableModule, MatTabsModule, MatToolbarModule, MatTooltipModule,
    MatTreeModule, BrowserAnimationsModule, CommonModule
  ],
  exports: [
    FontAwesomeModule, CdkTableModule, CdkTreeModule, MatAutocompleteModule,
    MatBadgeModule, MatBottomSheetModule, MatButtonModule, MatButtonToggleModule,
    MatCardModule, MatCheckboxModule, MatChipsModule, MatStepperModule,
    MatDatepickerModule, MatDialogModule, MatDividerModule, MatExpansionModule,
    MatGridListModule, MatIconModule, MatInputModule, MatListModule,
    MatMenuModule, MatNativeDateModule, MatPaginatorModule, MatProgressBarModule,
    MatProgressSpinnerModule, MatRadioModule, MatRippleModule, MatSelectModule,
    MatSidenavModule, MatSliderModule, MatSlideToggleModule, MatSnackBarModule,
    MatSortModule, MatTableModule, MatTabsModule, MatToolbarModule,
    MatTooltipModule, MatTreeModule, FlexLayoutModule, BrowserAnimationsModule,
    FormsModule, ReactiveFormsModule
  ],
  declarations: []
})
export class MaterialModule { }