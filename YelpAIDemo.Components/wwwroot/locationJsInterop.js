// This is a JavaScript module that is loaded on demand. It can export any number of
// functions, and may import other JavaScript modules if required.

export function getUserLocation() {
  return new Promise((resolve) => { 
    navigator.geolocation.getCurrentPosition(
      (position) => resolve(position),
      () => resolve(null)
    );
  });
}
export function initRoutesMap(origin, destination) {
    const mapElId = document.getElementById('map');
    console.log('Map element ID:', mapElId);
    const map = new google.maps.Map(mapElId, {
        zoom: 7,
        center: origin,             // Lat/Lng object literal is valid here
    });

    const service = new google.maps.DirectionsService();
    const renderer = new google.maps.DirectionsRenderer();

    service.route(
        {
            origin,
            destination,
            travelMode: google.maps.TravelMode.DRIVING, // or WALKING, etc.
        },
        (result, status) => {
            if (status === "OK") {
                renderer.setDirections(result);
            } else {
                console.error("Directions request failed: " + status);
            }
        }
    );
    console.log('Map:', map);
    renderer.setMap(map);
    renderer.setPanel(document.getElementById('directionsPanel'));
}
export function initLocationMap(locations) {
    const mapElId = document.getElementById('map');
    const map = new google.maps.Map(mapElId, {
        zoom: 15,
        center: locations[0],
    });
    locations.forEach(location => {
        new google.maps.AdvancedMarkerElement({
            position: location,
            map: map,
            title: location.title,
        });
    });

}

