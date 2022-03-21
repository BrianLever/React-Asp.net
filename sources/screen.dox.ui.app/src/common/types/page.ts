import H from 'history';

export interface IPage {
    history: H.History;
    location: H.Location;
    match: any;
}
