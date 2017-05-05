import { Component, Input, OnInit, AfterViewChecked, ChangeDetectionStrategy } from '@angular/core';
// import { Location } from '@angular/common';
import { FormGroup, AbstractControl, FormBuilder, Validators } from '@angular/forms';
// import { ToasterService, ToasterConfig } from 'angular2-toaster/angular2-toaster';

import { LPCReport } from '../models/lpcreport';
import { LPCReportService } from '../services/lpcreport';
import { ReferencesService } from '../../references/services/references';
import { SessionService } from '../../../shared/services/session';

@Component({
  selector: 'detail-form',
  templateUrl: 'app/routes/lpcreport/components/detailForm.html',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class DetailFormComponent implements OnInit, AfterViewChecked {
  // TOASTER
  toaster: any;
  // toasterConfig: any;
  // toasterconfig: ToasterConfig = new ToasterConfig({
  //   positionClass: 'toast-bottom-right',
  //   showCloseButton: true
  // });

  @Input() details: any;

  public objectTypes: Object;
  public boroughs: Object;

  public form: FormGroup;
  public name: AbstractControl;
  public objectType: AbstractControl;
  public architect: AbstractControl;
  public style: AbstractControl;
  public dateDesignated: AbstractControl;
  public street: AbstractControl;
  public borough: AbstractControl;

  public submitted: boolean = false;
  public formInitted: boolean = false;


  constructor(
    // private toasterService: ToasterService,
    private builder: FormBuilder,
    private session: SessionService,
    // private location: Location,
    private referenceService: ReferencesService,
    private lpcReportService: LPCReportService
    ) {
    this.form = this.builder.group({
      'name': ['', Validators.compose([Validators.required])],
      'objectType': ['', Validators.compose([Validators.required])],
      'architect': ['', Validators.compose([Validators.required])],
      'style': ['', Validators.compose([Validators.required])],
      'dateDesignated': ['', Validators.compose([Validators.required])],
      'street': ['', Validators.compose([Validators.required])],
      'borough': ['', Validators.compose([Validators.required])]
    });

    this.name = this.form.controls['name'];
    this.objectType = this.form.controls['objectType'];
    this.architect = this.form.controls['architect'];
    this.style = this.form.controls['style'];
    this.dateDesignated = this.form.controls['dateDesignated'];
    this.street = this.form.controls['street'];
    this.borough = this.form.controls['borough'];
  };

  getObjectTypes() {
    this.referenceService.getObjectTypes().subscribe(
      data => {
        this.objectTypes = data;
        this.session.set('objectTypes', data);
      },
      err => this.pop(err, 'Error', 'error')
    );
    return this.session.get('objectTypes');
  }

  getBoroughs() {
    this.referenceService.getBoroughs().subscribe(
      data => {
        this.boroughs = data;
        this.session.set('boroughs', data);
      },
      err => this.pop(err, 'Error', 'error')
    );
    return this.session.get('boroughs');
  }

  ngOnInit() {
    let objectTypes = this.session.get('objectTypes');
    this.objectTypes = (objectTypes) ? objectTypes : this.getObjectTypes();

    let boroughs = this.session.get('boroughs');
    this.boroughs = (boroughs) ? boroughs : this.getBoroughs();
  };

  ngAfterViewChecked() {
    if (this.details && !this.formInitted) {
      this.form.patchValue(this.details, { onlySelf: true });
      this.formInitted = true;
    }
  };

  // TOSATER METHOD
  pop(message: string, title ? : string, type ? : string) {
    this.toaster = {
      type: type || 'info',
      title: title || '',
      text: message
    };
    // this.toasterService.pop(this.toaster.type, this.toaster.title, this.toaster.text);
  };

  getDateFormatted(date) {
    var mm = date.getMonth() + 1; // getMonth() is zero-based
    var dd = date.getDate();
    return [(mm.length === 2 && '0') + mm, (dd.length === 2 && '0') + dd, date.getFullYear()].join('/'); // padding
  };

  public onSubmit(values: LPCReport) {
    this.submitted = true;
    if (this.form.valid) {
      let details: LPCReport = values;
      details.id = this.details.id;
      // console.log(values);
      // Submit http request
      this.lpcReportService.putLPCReport(this.details.id, values)
        .subscribe(
          (res) => {
            // console.log(res);
            this.pop(details.name + ' updated', '', 'success');
            // this.location.back();
          },
          err => this.pop(err, 'Error', 'error')
        );
    }
  }
};
