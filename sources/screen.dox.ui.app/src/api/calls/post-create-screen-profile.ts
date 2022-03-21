import axios from  '../axios';
import { IScreenProfileCreateRequestItem, IScreenProfilesResponseItem  } from '../../actions/screen-profiles';

const postCreateScreenProfile = async (props: IScreenProfileCreateRequestItem): Promise<IScreenProfilesResponseItem> => {
    const replace = !!props ? props : {};
    return await axios.instance.post(`screeningprofile`, {
        ...replace,
    });
}

export default postCreateScreenProfile;