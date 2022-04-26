import { Component, AfterViewInit, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { AlertifyService } from 'app/core/services/alertify.service';
import { LookUpService } from 'app/core/services/lookUp.service';
import { AuthService } from 'app/core/components/admin/login/services/auth.service';
import { Field } from './models/Field';
import { FieldService } from './services/Field.service';
import { environment } from 'environments/environment';

declare var jQuery: any;

@Component({
	selector: 'app-field',
	templateUrl: './field.component.html',
	styleUrls: ['./field.component.scss']
})
export class FieldComponent implements AfterViewInit, OnInit {
	
	dataSource: MatTableDataSource<any>;
	@ViewChild(MatPaginator) paginator: MatPaginator;
	@ViewChild(MatSort) sort: MatSort;
	displayedColumns: string[] = ['id','name', 'update','delete'];

	fieldList:Field[];
	field:Field=new Field();

	fieldAddForm: FormGroup;


	fieldId:number;

	constructor(private fieldService:FieldService, private lookupService:LookUpService,private alertifyService:AlertifyService,private formBuilder: FormBuilder, private authService:AuthService) { }

    ngAfterViewInit(): void {
        this.getFieldList();
    }

	ngOnInit() {

		this.createFieldAddForm();
	}


	getFieldList() {
		this.fieldService.getFieldList().subscribe(data => {
			this.fieldList = data;
			this.dataSource = new MatTableDataSource(data);
            this.configDataTable();
		});
	}

	save(){

		if (this.fieldAddForm.valid) {
			this.field = Object.assign({}, this.fieldAddForm.value)

			if (this.field.id == 0)
				this.addField();
			else
				this.updateField();
		}

	}

	addField(){

		this.fieldService.addField(this.field).subscribe(data => {
			this.getFieldList();
			this.field = new Field();
			jQuery('#field').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.fieldAddForm);

		})

	}

	updateField(){

		this.fieldService.updateField(this.field).subscribe(data => {

			var index=this.fieldList.findIndex(x=>x.id==this.field.id);
			this.fieldList[index]=this.field;
			this.dataSource = new MatTableDataSource(this.fieldList);
            this.configDataTable();
			this.field = new Field();
			jQuery('#field').modal('hide');
			this.alertifyService.success(data);
			this.clearFormGroup(this.fieldAddForm);

		})

	}

	createFieldAddForm() {
		this.fieldAddForm = this.formBuilder.group({		
			id : [0],
name : ["", Validators.required]
		})
	}

	deleteField(fieldId:number){
		this.fieldService.deleteField(fieldId).subscribe(data=>{
			this.alertifyService.success(data.toString());
			this.fieldList=this.fieldList.filter(x=> x.id!=fieldId);
			this.dataSource = new MatTableDataSource(this.fieldList);
			this.configDataTable();
		})
	}

	getFieldById(fieldId:number){
		this.clearFormGroup(this.fieldAddForm);
		this.fieldService.getFieldById(fieldId).subscribe(data=>{
			this.field=data;
			this.fieldAddForm.patchValue(data);
		})
	}


	clearFormGroup(group: FormGroup) {

		group.markAsUntouched();
		group.reset();

		Object.keys(group.controls).forEach(key => {
			group.get(key).setErrors(null);
			if (key == 'id')
				group.get(key).setValue(0);
		});
	}

	checkClaim(claim:string):boolean{
		return this.authService.claimGuard(claim)
	}

	configDataTable(): void {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	applyFilter(event: Event) {
		const filterValue = (event.target as HTMLInputElement).value;
		this.dataSource.filter = filterValue.trim().toLowerCase();

		if (this.dataSource.paginator) {
			this.dataSource.paginator.firstPage();
		}
	}

  }
