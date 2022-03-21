

export const setToken = (key: string, value: string | number, ttl: number) => {
	const now = new Date()

	// `item` is an object which contains the original value
	// as well as the time when it's supposed to expire
	var data;
	const item = {
		value: value,
		// expiry: now.getTime() + ttl,
	}

	data = item;
	
	// if(ttl === 0) {
	// 	const { expiry, ...other } = item;
	// 	data = other;
	// }

	localStorage.setItem(key, JSON.stringify(data))
}

export const getToken = (key: string) =>  {
	const itemStr = localStorage.getItem(key);
	// if the item doesn't exist, return null
	if (!itemStr) {
		return null
	}
	const item = JSON.parse(itemStr)
	const now = new Date()
	// compare the expiry time of the item with the current time
	// if (now.getTime() > item.expiry) {
	// 	// If the item is expired, delete the item from storage
	// 	// and return null
	// 	localStorage.removeItem(key)
	// 	return null
	// }
	return item.value
}

export const  Logout = () => {
	localStorage.clear();
}