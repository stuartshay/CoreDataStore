declare var $: any;
import { Component, ViewEncapsulation, HostBinding, OnInit } from '@angular/core';
import { SettingsService } from './core/settings/settings.service';

@Component({
  selector: 'app-root',
  template: '<router-outlet></router-outlet>',
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {

  @HostBinding('class.layout-fixed') get isFixed() {
    return this.settings.layout.isFixed; }
  @HostBinding('class.aside-collapsed') get isCollapsed() {
    return this.settings.layout.isCollapsed; }
  @HostBinding('class.layout-boxed') get isBoxed() {
    return this.settings.layout.isBoxed; }
  @HostBinding('class.layout-fs') get useFullLayout() {
    return this.settings.layout.useFullLayout; }
  @HostBinding('class.hidden-footer') get hiddenFooter() {
    return this.settings.layout.hiddenFooter; }
  @HostBinding('class.layout-h') get horizontal() {
    return this.settings.layout.horizontal; }
  @HostBinding('class.aside-float') get isFloat() {
    return this.settings.layout.isFloat; }
  @HostBinding('class.offsidebar-open') get offsidebarOpen() {
    return this.settings.layout.offsidebarOpen; }
  @HostBinding('class.aside-toggled') get asideToggled() {
    return this.settings.layout.asideToggled; }
  @HostBinding('class.aside-collapsed-text') get isCollapsedText() {
    return this.settings.layout.isCollapsedText; }

  constructor(public settings: SettingsService) {}

  ngOnInit() {
    $(document).on('click', '[href="#"]', e => e.preventDefault());
  }
}
