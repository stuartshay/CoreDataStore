import { Component, OnInit } from '@angular/core';

import { MapsService } from '../services/maps';

@Component({
  selector: 'app-google',
  providers: [MapsService],
  templateUrl: 'app/routes/maps/components/maps.html'
})
export class MapsComponent implements OnInit {

  public mapId: string;
  public maps: any = [];
  public markers: IMap[] = [];
  public features: any;

  public mapChanged(mapId: any) {
    this.mapsservice.getMapsId(mapId).subscribe(
      data => {
        this.features = data.features || [];
        let i: number = 0;
        this.markers = [];
        this.features.forEach(f => {
          let map: any = {};
          if (f.geometry && f.geometry.coordinates) {
            i++;
            map.lng = f.geometry.coordinates[0];
            map.lat = f.geometry.coordinates[1];
            map.label = String(i);
            this.markers.push(map);
          }
        });
      }
    );
  };

  zoom: number = 12;
  scrollwheel = true;
  constructor(private mapsservice: MapsService) {
    this.markers.push({ lat: 33.790807, lng: -117.835734, label: '' });
  }

  ngOnInit() {
    this.mapsservice.getMaps().subscribe(
      data => {
        this.maps = data || [];
        this.mapId = data[0].id;
        this.mapChanged(this.mapId);
      },
      err => console.log(err)
    );
  }
}

export interface IMap {
  lat: number;
  lng: number;
  label: string;
}
