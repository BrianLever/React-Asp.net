import React from 'react';
import classes from './checkbox.module.scss';

export type TRectangleCheckbox = {
    name: string;
    id: string;
    isChecked: boolean | undefined;
    changeHandler?: (v: any) => void;
    style?: any | null;
}

const CssrsCheckbox = (props: TRectangleCheckbox): React.ReactElement => {
    return (
        <label className={classes.container_cssrs} style={props.style}>
            <input type="checkbox" checked={props.isChecked} onChange={props.changeHandler} name={props.name} id={props.id} />
            <span className={classes.checkmark_cssrs}></span>
        </label>
    )
}

export default CssrsCheckbox;