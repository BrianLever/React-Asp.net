import axios from  '../axios';
import {  IScreeningProfileMinimunAgeResponseItem  } from '../../actions/screen-profiles';

const updateScreenProfileMinimumAgeList = async (id: number, props: { Items: Array<IScreeningProfileMinimunAgeResponseItem>} )  => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`screeningprofile/${id}/age`, {
        ...replace,
    });
}

export default updateScreenProfileMinimumAgeList;