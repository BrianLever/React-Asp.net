import axios from  '../axios';
import { IScreenProfileCreateRequestItem, IScreenProfilesResponseItem  } from '../../actions/screen-profiles';

const UpdateScreenProfile = async (id: number, props: IScreenProfileCreateRequestItem)  => {
    const replace = !!props ? props : {};
    return await axios.instance.put(`screeningprofile/`+id, {
        ...replace,
    });
}

export default UpdateScreenProfile;