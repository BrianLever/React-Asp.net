import React from 'react';
import classes from './checkbox.module.scss';

export type TSearchCheckboxProps = {
    name: string;
    id: string;
    isChecked: boolean;
    changeHandler?: (v: any) => void;
}

const SearchCheckbox = (props: TSearchCheckboxProps): React.ReactElement => {
    return (
        <label className={classes.container}>
            <input 
                type="radio"                 
                checked={props.isChecked}
                name={props.name}
                id={props.id}
                readOnly
                style={{ width: '20px', height: '20px', margin: '2px'}}
            />
            <span 
                className={classes.checkmark} 
                onClick={e => {
                    props.changeHandler && props.changeHandler(e);
                }}
            />
        </label>
    )
}

export default SearchCheckbox;