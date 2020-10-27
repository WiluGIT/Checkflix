import { IUserPreferencesViewModel } from './../ClientViewModels/IUserPreferencesViewModel';
import { UserPreferencesService } from './../../services/user-preferences.service';
import { CategoryService } from './../../services/category.service';
import { VodService } from './../../services/vod.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ICategoryViewModel } from '../ClientViewModels/ICategoryViewModel';
import { IVodViewModel } from '../ClientViewModels/IVodViewModel';

@Component({
  selector: 'app-user-preferences',
  templateUrl: './user-preferences.component.html',
  styleUrls: ['./user-preferences.component.css']
})
export class UserPreferencesComponent implements OnInit {
  preferencesForm: FormGroup;
  categoryList: ICategoryViewModel[];
  vodList: IVodViewModel[];
  userPreferences: IUserPreferencesViewModel;
  constructor(private fb: FormBuilder,
    private vodService: VodService,
    private categoryService: CategoryService,
    private userPreferencesService: UserPreferencesService) { }

  ngOnInit() {
    this.preferencesForm = this.fb.group({
      categories: ['', [
      ]],
      vods: ['', [
      ]]
    });
    // populate vods and categories to select boxes
    this.vodService
      .getVods()
      .subscribe(vods => this.vodList = vods);

    this.categoryService
      .getCategories()
      .subscribe(categories => this.categoryList = categories);

    this.userPreferencesService
      .getPreferences()
      .subscribe(preferences => {
        this.userPreferences = preferences;
        const selectedCategories = this.userPreferences.categories.map(cat => cat.categoryName);
        const selectedVods = this.userPreferences.vods.map(vod => vod.platformName);
        this.preferencesForm.controls.categories.setValue(selectedCategories);
        this.preferencesForm.controls.vods.setValue(selectedVods);
      });
  }
}
